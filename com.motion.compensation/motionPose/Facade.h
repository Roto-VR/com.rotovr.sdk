#pragma once

#define DLL_EXPORT extern "C" __declspec(dllexport) 
#include "MotionPoseController.h"
#include "third-party/easylogging++/easylogging++.h"

namespace driver {
	class Facade
	{

	public:Facade() {
		
	}

		  // MMF: OVRMC	
		  LPCWSTR szName_MC = L"Local\\OVRMC_MMFv1";
		  char* adressFile_MC = nullptr;
		  HANDLE MapFile_MC = NULL;
		  MMFstruct_OVRMC_v1* Data_OVRMC = nullptr;

		  // MMF: FlyPT Mover	
		  LPCWSTR szName_Mover = L"Local\\motionRigPose";
		   char* adressFile_Mover = nullptr;	 
		  HANDLE MapFile_Mover = NULL;	
		  MMFstruct_Mover_v1* Data_Mover = nullptr;
		 

		  void StartSession();
		  void StopSession();
		  void InitOffset(double x, double y);
		  void UpdateAngle(INT32 angle);
	};

	Facade* staticObject;

	DLL_EXPORT void InitFacade() {
		staticObject = new Facade();
		LOG(TRACE) << "Init Facade";
	}

	DLL_EXPORT void Start() {

		LOG(TRACE) << "Start Session";
		staticObject->StartSession();
	}

	DLL_EXPORT void InitOffset(double x, double y) {
		LOG(TRACE) << "Init Offset";

		staticObject->InitOffset(x, y);
	}

	DLL_EXPORT void UpdateAngle(INT32 angle) {
		LOG(TRACE) << "Run Frame" << angle;
		staticObject->UpdateAngle(angle);
	}

	DLL_EXPORT void Stop() {
		LOG(TRACE) << "Stop Session";
		staticObject->StopSession();
	}
}

