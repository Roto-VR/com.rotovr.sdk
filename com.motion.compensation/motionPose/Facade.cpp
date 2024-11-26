#include "Facade.h"
#include "third-party/easylogging++/easylogging++.h"

namespace driver {

	void Facade::StartSession()
	{
		if (MapFile_MC == NULL) {
			MapFile_MC = CreateFileMapping(
				INVALID_HANDLE_VALUE,
				NULL,
				PAGE_READWRITE,
				0,
				4096,
				szName_MC);
			if (MapFile_MC == NULL) {
				return;
			}

			adressFile_MC = (char*)MapViewOfFile(MapFile_MC, FILE_MAP_ALL_ACCESS, 0, 0, 4096);
			Data_OVRMC = (MMFstruct_OVRMC_v1*)adressFile_MC;
		}

		if (MapFile_Mover == NULL) {
			MapFile_Mover = CreateFileMapping(
				INVALID_HANDLE_VALUE,
				NULL,
				PAGE_READWRITE,
				0,
				4096,
				szName_Mover);

			if (MapFile_Mover == NULL) {
				return;
			}

			adressFile_Mover = (char*)MapViewOfFile(MapFile_Mover, FILE_MAP_ALL_ACCESS, 0, 0, 4096);
			Data_Mover = (MMFstruct_Mover_v1*)adressFile_Mover;

			Data_Mover->rigYaw = 25;
		}

		LOG(TRACE) << "Fasade Start Session";
	}
	void Facade::StopSession()
	{

		if (CloseHandle(MapFile_Mover) == 0)
		{
			LOG(ERROR) << "Failed to close Mover handle! Error: " << GetLastError();
		}

		if (CloseHandle(MapFile_MC) == 0)
		{
			LOG(ERROR) << "Failed to close MC handle! Error: " << GetLastError();
		}
	}
	void Facade::InitOffset(double x, double y)
	{
		if (MapFile_MC == NULL)
			StartSession();

		Data_OVRMC->Translation.v[0] = x;
		Data_OVRMC->Translation.v[1] = y;
	}
	void Facade::UpdateAngle(INT32 angle)
	{
		Data_Mover->rigYaw = angle;
	}
}
