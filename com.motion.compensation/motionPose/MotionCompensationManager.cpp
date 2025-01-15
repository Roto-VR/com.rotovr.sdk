#include "MotionCompensationManager.h"

#include "DeviceManipulationHandle.h"
#include "ServerDriver.h"

#include <cmath>
#include <boost/math/constants/constants.hpp>
#include <boost/interprocess/shared_memory_object.hpp>


namespace driver
{
	MotionCompensationManager::MotionCompensationManager(ServerDriver* parent) : m_parent(parent)
	{
		_Offset = {};
	}

	bool MotionCompensationManager::openMmf(HANDLE& MapFile, char*& mmfFile, LPCWSTR szName, int BufferSize, bool& Connected)
	{
		MapFile = OpenFileMapping(FILE_MAP_ALL_ACCESS, FALSE, szName);

		// return if MMF could not be opened
		if (MapFile == NULL)
		{
			return false;
		}

		mmfFile = (char*)MapViewOfFile(MapFile, FILE_MAP_READ, 0, 0, BufferSize);

		// return if view could not be mapped
		if (mmfFile == NULL)
		{
			CloseHandle(MapFile);
			return false;
		}

		Connected = true;

		LOG(INFO) << "Successfully connected to " << szName;

		return true;
	}

	bool MotionCompensationManager::setMotionCompensationMode(MotionCompensationMode Mode, int McDevice, int RtDevice)
	{
		if (Mode == MotionCompensationMode::ReferenceTracker)
			LOG(INFO) << "setMotionCompensationMode to ReferenceTracker   McDevice: " << McDevice << "  RtDevice: " << RtDevice;
		else
			LOG(INFO) << "setMotionCompensationMode to Disabled   McDevice: " << McDevice << "  RtDevice: " << RtDevice;

		if (Mode == MotionCompensationMode::ReferenceTracker)
		{
			_RefPoseValid = false;
			_RefPoseValidCounter = 0;
			_ZeroPoseValid = false;
			_Enabled = true;

			setAlpha(_Samples);

		}
		else
		{
			_Enabled = false;
		}

		_McDeviceID = McDevice;
		_RtDeviceID = RtDevice;
		_Mode = Mode;

		return true;
	}

	void MotionCompensationManager::setNewMotionCompensatedDevice(int MCdevice)
	{
		_McDeviceID = MCdevice;
	}

	void MotionCompensationManager::setNewReferenceTracker(int RTdevice)
	{
		_RtDeviceID = RTdevice;
		_RefPoseValid = false;
		_ZeroPoseValid = false;
	}

	void MotionCompensationManager::setAlpha(uint32_t samples)
	{
		_Samples = samples;
		_Alpha = 2.0 / (1.0 + (double)samples);
	}

	void MotionCompensationManager::setZeroMode(bool setZero)
	{
		_SetZeroMode = setZero;

		_zeroVec(_RefVel);
		_zeroVec(_RefRotVel);
		_zeroVec(_RefAcc);
		_zeroVec(_RefRotAcc);
	}

	void MotionCompensationManager::setOffsets(MMFstruct_Mover_v1* mover)
	{


	}

	bool MotionCompensationManager::isZeroPoseValid()
	{
		return _ZeroPoseValid;
	}

	void MotionCompensationManager::resetZeroPose()
	{
		_ZeroPoseValid = false;
	}

	void MotionCompensationManager::setZeroPose(const vr::DriverPose_t& pose)
	{
		// convert pose from driver space to app space
		vr::HmdQuaternion_t tmpConj = vrmath::quaternionConjugate(pose.qWorldFromDriverRotation);

		// Save zero points
		_ZeroLock.lock();
		_ZeroPos = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, pose.vecPosition, false) + pose.vecWorldFromDriverTranslation;
		_ZeroRot = pose.qWorldFromDriverRotation * pose.qRotation;

		_ZeroPoseValid = true;
		_ZeroLock.unlock();
	}

	void MotionCompensationManager::updateRefPose(const vr::DriverPose_t& pose)
	{
		// From https://github.com/ValveSoftware/driver_hydra/blob/master/drivers/driver_hydra/driver_hydra.cpp Line 835:
			// "True acceleration is highly volatile, so it's not really reasonable to
			// extrapolate much from it anyway.  Passing it as 0 from any driver should
			// be fine."

			// Line 832:
			// "The trade-off here is that setting a valid velocity causes the controllers
			// to jitter, but the controllers feel much more "alive" and lighter.
			// The jitter while stationary is more annoying than the laggy feeling caused
			// by disabling velocity (which effectively disables prediction for rendering)."
			// That means that we have to calculate the velocity to not interfere with the prediction for rendering

			// Oculus devices do use acceleration. It also seems that the HMD uses theses values for render-prediction

		vr::HmdVector3d_t Filter_vecPosition = { 0, 0, 0 };
		vr::HmdVector3d_t Filter_vecVelocity = { 0, 0, 0 };
		vr::HmdVector3d_t Filter_vecAcceleration = { 0, 0, 0 };
		vr::HmdVector3d_t Filter_vecAngularVelocity = { 0, 0, 0 };
		vr::HmdVector3d_t Filter_vecAngularAcceleration = { 0, 0, 0 };
		vr::HmdVector3d_t RotEulerFilter = { 0, 0, 0 };

		vr::HmdQuaternion_t tmpConj = vrmath::quaternionConjugate(pose.qWorldFromDriverRotation);

		// Get current time in microseconds and convert it to seconds
		long long now = std::chrono::duration_cast <std::chrono::microseconds>(std::chrono::system_clock::now().time_since_epoch()).count();
		double tdiff = (double)(now - _RefTrackerLastTime) / 1.0E6 + (pose.poseTimeOffset - _RefTrackerLastPose.poseTimeOffset);

		// Position
		// Add a exponential median average filter
		if (_Samples >= 2)
		{
			// ----------------------------------------------------------------------------------------------- //
			// ----------------------------------------------------------------------------------------------- //
			// Position
			Filter_vecPosition.v[0] = DEMA(pose.vecPosition[0], 0);
			Filter_vecPosition.v[1] = DEMA(pose.vecPosition[1], 1);
			Filter_vecPosition.v[2] = DEMA(pose.vecPosition[2], 2);

			// ----------------------------------------------------------------------------------------------- //
			// ----------------------------------------------------------------------------------------------- //
			// Velocity and acceleration
			if (!_SetZeroMode)
			{
				Filter_vecVelocity.v[0] = vecVelocity(tdiff, Filter_vecPosition.v[0], _RefTrackerLastPose.vecPosition[0]);
				Filter_vecVelocity.v[1] = vecVelocity(tdiff, Filter_vecPosition.v[1], _RefTrackerLastPose.vecPosition[1]);
				Filter_vecVelocity.v[2] = vecVelocity(tdiff, Filter_vecPosition.v[2], _RefTrackerLastPose.vecPosition[2]);

				Filter_vecAcceleration.v[0] = vecAcceleration(tdiff, Filter_vecVelocity.v[0], _RefTrackerLastPose.vecVelocity[0]);
				Filter_vecAcceleration.v[1] = vecAcceleration(tdiff, Filter_vecVelocity.v[1], _RefTrackerLastPose.vecVelocity[1]);
				Filter_vecAcceleration.v[2] = vecAcceleration(tdiff, Filter_vecVelocity.v[2], _RefTrackerLastPose.vecVelocity[2]);
			}
		}
		else
		{
			_copyVec(Filter_vecPosition, pose.vecPosition);
			_copyVec(Filter_vecVelocity, pose.vecVelocity);
		}

		// convert pose from driver space to app space
		_RefLock.lock();
		_RefPos = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, Filter_vecPosition, false) + pose.vecWorldFromDriverTranslation;
		_RefLock.unlock();

		// ----------------------------------------------------------------------------------------------- //
		// ----------------------------------------------------------------------------------------------- //
		// Rotation
		if (_LpfBeta <= 0.9999)
		{
			// 1st stage
			_Filter_rotPosition[0] = lowPassFilterQuaternion(pose.qRotation, _Filter_rotPosition[0]);

			// 2nd stage
			_Filter_rotPosition[1] = lowPassFilterQuaternion(_Filter_rotPosition[0], _Filter_rotPosition[1]);


			vr::HmdVector3d_t RotEulerFilter = toEulerAngles(_Filter_rotPosition[1]);

			if (!_SetZeroMode)
			{
				Filter_vecAngularVelocity.v[0] = rotVelocity(tdiff, RotEulerFilter.v[0], _RotEulerFilterOld.v[0]);
				Filter_vecAngularVelocity.v[1] = rotVelocity(tdiff, RotEulerFilter.v[1], _RotEulerFilterOld.v[1]);
				Filter_vecAngularVelocity.v[2] = rotVelocity(tdiff, RotEulerFilter.v[2], _RotEulerFilterOld.v[2]);

				Filter_vecAngularAcceleration.v[0] = vecAcceleration(tdiff, Filter_vecAngularVelocity.v[0], _RefTrackerLastPose.vecAngularVelocity[0]);
				Filter_vecAngularAcceleration.v[1] = vecAcceleration(tdiff, Filter_vecAngularVelocity.v[1], _RefTrackerLastPose.vecAngularVelocity[1]);
				Filter_vecAngularAcceleration.v[2] = vecAcceleration(tdiff, Filter_vecAngularVelocity.v[2], _RefTrackerLastPose.vecAngularVelocity[2]);
			}
		}
		else
		{
			_Filter_rotPosition[1] = pose.qRotation;

			_copyVec(Filter_vecAngularVelocity, pose.vecAngularVelocity);
			_copyVec(Filter_vecAngularAcceleration, pose.vecAngularAcceleration);
		}

		// calculate orientation difference and its inverse
		vr::HmdQuaternion_t poseWorldRot = pose.qWorldFromDriverRotation * _Filter_rotPosition[1];

		_RefLock.lock();
		_ZeroLock.lock();
		_RefRot = poseWorldRot * vrmath::quaternionConjugate(_ZeroRot);
		_RefRotInv = vrmath::quaternionConjugate(_RefRot);
		_ZeroLock.unlock();
		_RefLock.unlock();

		if (!_SetZeroMode)
		{
			// Convert velocity and acceleration values into app space
			_RefVelLock.lock();
			_RefVel = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, Filter_vecVelocity, false);
			_RefRotVel = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, Filter_vecAngularVelocity, false);

			_RefAcc = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, Filter_vecAcceleration, false);
			_RefRotAcc = vrmath::quaternionRotateVector(pose.qWorldFromDriverRotation, tmpConj, Filter_vecAngularAcceleration, false);
			_RefVelLock.unlock();
		}

		// ----------------------------------------------------------------------------------------------- //
		// ----------------------------------------------------------------------------------------------- //
		// Wait 100 frames before setting reference pose to valid
		if (_RefPoseValidCounter > 100)
		{
			_RefPoseValid = true;
		}
		else
		{
			_RefPoseValidCounter++;
		}

		// Save last rotation and pose
		_RotEulerFilterOld = RotEulerFilter;
		_RefTrackerLastPose = pose;
	}

	bool MotionCompensationManager::applyMotionCompensation(vr::DriverPose_t& pose)
	{
		// Apply motion data
		if (Data_RigPose != nullptr) {

			auto newChairAngle = Data_RigPose->rigYaw;
			_ChairRotation = vrmath::quaternionFromYawRollPitch(newChairAngle * 0.01745329251994329, 0, 0);

			if (_PrevAngle != newChairAngle) {

				LOG(INFO) << "Compensated [ w: " << _ChairRotation.w << " x: " << _ChairRotation.x << " y: " << _ChairRotation.y << " z: " << _ChairRotation.z << " ] For Angle: " << newChairAngle;
				_PrevAngle = newChairAngle;
			}

			// Convert pose from driver space to app space		
			vr::HmdQuaternion_t tmpConj = vrmath::quaternionConjugate(pose.qWorldFromDriverRotation);

			// Do motion compensation
			vr::HmdQuaternion_t poseWorldRot = pose.qWorldFromDriverRotation * pose.qRotation;

			_RefLock.lock();
			_ZeroLock.lock();
			vr::HmdQuaternion_t compensatedPoseWorldRot = _ChairRotation * poseWorldRot;
			_ZeroLock.unlock();
			_RefLock.unlock();
			// convert back to driver space

			pose.qRotation = tmpConj * compensatedPoseWorldRot;
		}

		return true;
	}

	void MotionCompensationManager::runFrame()
	{
		try
		{

			if (!_moverConnected)
			{
				// Create connection to OVRMC
				if (openMmf(MapFile_Mover, mmfFile_Mover, L"Local\\motionRigPose", 4096, _moverConnected))
				{
					Data_RigPose = (MMFstruct_Mover_v1*)mmfFile_Mover;
				}
			}
		}
		catch (std::exception& e)
		{
			_moverConnected = false;
		}
	}

	double MotionCompensationManager::vecVelocity(double time, const double vecPosition, const double Old_vecPosition)
	{
		double NewVelocity = 0.0;

		if (time != (double)0.0)
		{
			NewVelocity = (vecPosition - Old_vecPosition) / time;
		}

		return NewVelocity;
	}

	double MotionCompensationManager::vecAcceleration(double time, const double vecVelocity, const double Old_vecVelocity)
	{
		double NewAcceleration = 0.0;

		if (time != (double)0.0)
		{
			NewAcceleration = (vecVelocity - Old_vecVelocity) / time;
		}

		return NewAcceleration;
	}

	double MotionCompensationManager::rotVelocity(double time, const double vecAngle, const double Old_vecAngle)
	{
		double NewVelocity = 0.0;

		if (time != (double)0.0)
		{
			NewVelocity = (1 - angleDifference(vecAngle, Old_vecAngle)) / time;
		}

		return NewVelocity;
	}

	// Low Pass Filter for 3d Vectors
	double MotionCompensationManager::DEMA(const double RawData, int Axis)
	{
		_Filter_vecPosition[0].v[Axis] += _Alpha * (RawData - _Filter_vecPosition[1].v[Axis]);
		_Filter_vecPosition[1].v[Axis] += _Alpha * (_Filter_vecPosition[0].v[Axis] - _Filter_vecPosition[1].v[Axis]);
		return 2 * _Filter_vecPosition[0].v[Axis] - _Filter_vecPosition[1].v[Axis];
	}

	// Low Pass Filter for 3d Vectors
	vr::HmdVector3d_t MotionCompensationManager::LPF(const double RawData[3], vr::HmdVector3d_t SmoothData)
	{
		vr::HmdVector3d_t RetVal;

		RetVal.v[0] = SmoothData.v[0] - (_LpfBeta * (SmoothData.v[0] - RawData[0]));
		RetVal.v[1] = SmoothData.v[1] - (_LpfBeta * (SmoothData.v[1] - RawData[1]));
		RetVal.v[2] = SmoothData.v[2] - (_LpfBeta * (SmoothData.v[2] - RawData[2]));

		return RetVal;
	}

	// Low Pass Filter for 3d Vectors
	vr::HmdVector3d_t MotionCompensationManager::LPF(vr::HmdVector3d_t RawData, vr::HmdVector3d_t SmoothData)
	{
		vr::HmdVector3d_t RetVal;

		RetVal.v[0] = SmoothData.v[0] - (_LpfBeta * (SmoothData.v[0] - RawData.v[0]));
		RetVal.v[1] = SmoothData.v[1] - (_LpfBeta * (SmoothData.v[1] - RawData.v[1]));
		RetVal.v[2] = SmoothData.v[2] - (_LpfBeta * (SmoothData.v[2] - RawData.v[2]));

		return RetVal;
	}

	// Low Pass Filter for quaternion
	vr::HmdQuaternion_t MotionCompensationManager::lowPassFilterQuaternion(vr::HmdQuaternion_t RawData, vr::HmdQuaternion_t SmoothData)
	{
		return slerp(SmoothData, RawData, _LpfBeta);
	}

	// Spherical Linear Interpolation for Quaternions
	vr::HmdQuaternion_t MotionCompensationManager::slerp(vr::HmdQuaternion_t q1, vr::HmdQuaternion_t q2, double lambda)
	{
		vr::HmdQuaternion_t qr;

		double dotproduct = q1.x * q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;

		// if q1 and q2 are the same, we can return either of the values
		if (dotproduct >= 1.0 || dotproduct <= -1.0)
		{
			return q1;
		}

		double theta, st, sut, sout, coeff1, coeff2;

		// algorithm adapted from Shoemake's paper
		lambda = lambda / 2.0;

		theta = (double)acos(dotproduct);
		if (theta < 0.0) theta = -theta;

		st = (double)sin(theta);
		sut = (double)sin(lambda * theta);
		sout = (double)sin((1 - lambda) * theta);
		coeff1 = sout / st;
		coeff2 = sut / st;

		qr.x = coeff1 * q1.x + coeff2 * q2.x;
		qr.y = coeff1 * q1.y + coeff2 * q2.y;
		qr.z = coeff1 * q1.z + coeff2 * q2.z;
		qr.w = coeff1 * q1.w + coeff2 * q2.w;


		//Normalize
		double norm = sqrt(qr.x * qr.x + qr.y * qr.y + qr.z * qr.z + qr.w * qr.w);
		qr.x /= norm;
		qr.y /= norm;
		qr.z /= norm;

		return qr;
	}

	// Convert Quaternion to Euler Angles in Radians
	vr::HmdVector3d_t MotionCompensationManager::toEulerAngles(vr::HmdQuaternion_t q)
	{
		vr::HmdVector3d_t angles;

		// roll (x-axis rotation)
		double sinr_cosp = 2 * (q.w * q.x + q.y * q.z);
		double cosr_cosp = 1 - 2 * (q.x * q.x + q.y * q.y);
		angles.v[0] = std::atan2(sinr_cosp, cosr_cosp);

		// pitch (y-axis rotation)
		double sinp = 2 * (q.w * q.y - q.z * q.x);

		if (std::abs(sinp) >= 1)
		{
			angles.v[1] = std::copysign(boost::math::constants::pi<double>() / 2, sinp); // use 90 degrees if out of range
		}
		else
		{
			angles.v[1] = std::asin(sinp);
		}

		// yaw (z-axis rotation)
		double siny_cosp = 2 * (q.w * q.z + q.x * q.y);
		double cosy_cosp = 1 - 2 * (q.y * q.y + q.z * q.z);
		angles.v[2] = std::atan2(siny_cosp, cosy_cosp);

		return angles;
	}

	// Returns the shortest difference between to angles
	const double MotionCompensationManager::angleDifference(double Raw, double New)
	{
		double diff = fmod((New - Raw + (double)180), (double)360) - (double)180;
		return diff < -(double)180 ? diff + (double)360 : diff;
	}

	vr::HmdVector3d_t MotionCompensationManager::transform(vr::HmdVector3d_t VecRotation, vr::HmdVector3d_t VecPosition, vr::HmdVector3d_t point)
	{
		// point is the user-input offset to the controller
		// VecRotation and VecPosition is the current reference-pose (Controller or input from Mover)
		vr::HmdQuaternion_t quat = vrmath::quaternionFromYawPitchRoll(VecRotation.v[0], VecRotation.v[1], VecRotation.v[2]);
		return transform(quat, VecPosition, point);
	}

	// Calculates the new coordinates of 'point', moved and rotated by VecRotation and VecPosition
	vr::HmdVector3d_t MotionCompensationManager::transform(vr::HmdQuaternion_t quat, vr::HmdVector3d_t VecPosition, vr::HmdVector3d_t point)
	{
		vr::HmdVector3d_t translation = vrmath::quaternionRotateVector(quat, VecPosition);

		return vrmath::quaternionRotateVector(quat, point) + translation;
	}

	// 
	vr::HmdVector3d_t MotionCompensationManager::transform(vr::HmdVector3d_t VecRotation, vr::HmdVector3d_t VecPosition, vr::HmdVector3d_t centerOfRotation, vr::HmdVector3d_t point)
	{
		// point is the user-input offset to the controller
		// VecRotation and VecPosition is the current rig-pose

		vr::HmdQuaternion_t quat = vrmath::quaternionFromYawPitchRoll(VecRotation.v[0], VecRotation.v[1], VecRotation.v[2]);

		double n1 = quat.x * 2.f;
		double n2 = quat.y * 2.f;
		double n3 = quat.z * 2.f;

		double _n4 = quat.x * n1;
		double _n5 = quat.y * n2;
		double _n6 = quat.z * n3;
		double _n7 = quat.x * n2;
		double _n8 = quat.x * n3;
		double _n9 = quat.y * n3;
		double _n10 = quat.w * n1;
		double _n11 = quat.w * n2;
		double _n12 = quat.w * n3;

		vr::HmdVector3d_t translation = {
			(1 - (_n5 + _n6)) * (VecPosition.v[0]) + (_n7 - _n12) * (VecPosition.v[1]) + (_n8 + _n11) * (VecPosition.v[2]),
			(_n7 + _n12) * (VecPosition.v[0]) + (1 - (_n4 + _n6)) * (VecPosition.v[1]) + (_n9 - _n10) * (VecPosition.v[2]),
			(_n8 - _n11) * (VecPosition.v[0]) + (_n9 + _n10) * (VecPosition.v[1]) + (1 - (_n4 + _n5)) * (VecPosition.v[2])
		};

		return {
			(1.0 - (_n5 + _n6)) * (point.v[0] - centerOfRotation.v[0]) + (_n7 - _n12) * (point.v[1] - centerOfRotation.v[1]) + (_n8 + _n11) * (point.v[2] - centerOfRotation.v[2]) + centerOfRotation.v[0] + translation.v[0],
			(_n7 + _n12) * (point.v[0] - centerOfRotation.v[0]) + (1.0 - (_n4 + _n6)) * (point.v[1] - centerOfRotation.v[1]) + (_n9 - _n10) * (point.v[2] - centerOfRotation.v[2]) + centerOfRotation.v[1] + translation.v[1],
			(_n8 - _n11) * (point.v[0] - centerOfRotation.v[0]) + (_n9 + _n10) * (point.v[1] - centerOfRotation.v[1]) + (1.0 - (_n4 + _n5)) * (point.v[2] - centerOfRotation.v[2]) + centerOfRotation.v[2] + translation.v[2]
		};
	}
}

