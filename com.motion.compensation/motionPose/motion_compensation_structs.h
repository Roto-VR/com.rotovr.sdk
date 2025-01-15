#pragma once

#include <windows.h>
#include "openvr_driver.h"
#include "openvr_math.h"


// struct for OVRMC MMF version 1
struct MMFstruct_OVRMC_v1
{
	vr::HmdVector3d_t Translation;
	vr::HmdVector3d_t Rotation;
	vr::HmdQuaternion_t QRotation;
	uint32_t Flags_1;
	uint32_t Flags_2;
	double Reserved_double[10];
	int Reserved_int[10];

	MMFstruct_OVRMC_v1()
	{
		Translation = { 0, 0, 0 };
		Rotation = { 0, 0, 0 };
		QRotation = { 0, 0, 0, 0 };
		Flags_1 = 0;
		Flags_2 = 0;
	}
};

// struct for FlyPT Mover version 1
struct MMFstruct_Mover_v1
{
	double rigSway;
	double rigSurge;
	double rigHeave;
	double rigYaw;
	double rigRoll;
	double rigPitch;
};

enum class MotionCompensationMode : uint32_t
{
	Disabled = 0,
	ReferenceTracker = 1,
};

enum class MotionCompensationDeviceMode : uint32_t
{
	Default = 0,
	ReferenceTracker = 1,
	MotionCompensated = 2,
};

struct DeviceInfo
{
	uint32_t OpenVRId;
	vr::ETrackedDeviceClass deviceClass;
	MotionCompensationDeviceMode deviceMode;
};

