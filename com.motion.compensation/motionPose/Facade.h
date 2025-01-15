#pragma once

#define DLL_EXPORT extern "C" __declspec(dllexport) 
#include "motion_compensation_structs.h"
#include "easylogging++.h"

namespace driver {
	class Facade
	{

	public:Facade() {

	}
		  // MMF: FlyPT Mover	
		  LPCWSTR szName_Mover = L"Local\\motionRigPose";
		  char* adressFile_Mover = nullptr;
		  HANDLE MapFile_Mover = NULL;
		  MMFstruct_Mover_v1* Data_Mover = nullptr;


		  void StartSession();
		  void StopSession();		 
		  void UpdateAngle(INT32 angle);
	};

	Facade* staticObject;

	DLL_EXPORT void InitFacade() {
		staticObject = new Facade();
		LOG(INFO) << "Init Facade";
	}

	DLL_EXPORT void Start() {

		LOG(INFO) << "Start Session";
		staticObject->StartSession();
	}

	DLL_EXPORT void UpdateAngle(INT32 angle) {
		staticObject->UpdateAngle(angle);
	}

	DLL_EXPORT void Stop() {
		LOG(INFO) << "Stop Session";
		staticObject->StopSession();
	}
}

