#pragma once

#include "MotionPoseController.h"
#define API_DLL_EXPORT extern "C" __declspec(dllexport) 

namespace driver
{
	class MotionFacade
	{
	  public:  CMotionPoseControllerDriver Driver;
	};

	MotionFacade* staticFacade;

	API_DLL_EXPORT void InitFacade();
	API_DLL_EXPORT void InitOffset(double* x, double* y);
	API_DLL_EXPORT void Start();
	API_DLL_EXPORT void Stop();
	API_DLL_EXPORT void RunFrame(double* yaw);
}

