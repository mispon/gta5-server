import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

namespace RacecourseManager {
    let cylinder: LocalHandle
    let finishFlag: LocalHandle

    ServerEventHandler.getInstance().on('ShowRacecourseFinish', showFinish)
    ServerEventHandler.getInstance().on('HideRacecourseFinish', hideFinish)

    function showFinish() {
        cylinder = API.createMarker(1, new Vector3(1118.15, 193.02, 80.87), new Vector3(), new Vector3(), new Vector3(20, 20, 6), 66, 209, 244, 100)
        finishFlag = API.createMarker(4, new Vector3(1118.15, 193.02, 85.0), new Vector3(), new Vector3(0.0, 0.0, 146.85), new Vector3(5, 5, 5), 255, 255, 255, 150)
    }

    function hideFinish() {
       if (cylinder) {
           API.deleteEntity(cylinder)
           API.deleteEntity(finishFlag)
       }
    }
}