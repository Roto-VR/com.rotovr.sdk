#include "Facade.h"
#include "easylogging++.h"

namespace driver {

	void Facade::StartSession()
	{
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


			LOG(INFO) << "Open MapFile_Mover";


			adressFile_Mover = (char*)MapViewOfFile(MapFile_Mover, FILE_MAP_ALL_ACCESS, 0, 0, 4096);
			Data_Mover = (MMFstruct_Mover_v1*)adressFile_Mover;

			Data_Mover->rigYaw = 0;
		}

		LOG(INFO) << "Fasade Start Session";
	}
	void Facade::StopSession()
	{
		if (MapFile_Mover != NULL) {
			if (CloseHandle(MapFile_Mover) == 0)
			{
				LOG(ERROR) << "Failed to close Mover handle! Error: " << GetLastError();
			}
			else {
				MapFile_Mover = NULL;
			}
		}
	}
	void Facade::UpdateAngle(INT32 angle)
	{
		if (MapFile_Mover != NULL)
			Data_Mover->rigYaw = angle;
	}
}
