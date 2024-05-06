#include "third-party/easylogging++/easylogging++.h"
#include "MotionFacade.h"
#include "logging.h"



namespace driver
{
	API_DLL_EXPORT void InitFacade() {	
		staticFacade = new MotionFacade();
		LOG(INFO) << "Init Facade";
	}

	API_DLL_EXPORT void InitOffset(double* x, double* y) {
		LOG(INFO) << "InitOffset";
	}
	API_DLL_EXPORT void Start() {
		LOG(INFO) << "Start";
	}
	API_DLL_EXPORT void Stop() {
		LOG(INFO) << "Stop";
	}
	API_DLL_EXPORT void RunFrame(double* yaw) {
		LOG(INFO) << "Run";
	}
}