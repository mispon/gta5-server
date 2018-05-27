import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

namespace RaceManager {
    let currentMarker: LocalHandle
    let finishMarker: LocalHandle 

    ServerEventHandler.getInstance().on('ShowRacePoint', showNextPoint)
    ServerEventHandler.getInstance().on('HideRacePoint', hideCurrentPoint)

    function showNextPoint(args: any[]): void {
        hideCurrentPoint()  
        let position = args[0] as Vector3
        let dimension = args[1] as number        
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(7, 7, 10), 240, 247, 22, 170)
        if (args[2]) {
            let finishPosition = position.Add(new Vector3(0, 0, 8))
            let rotation = args[2] ? args[2] as Vector3 : new Vector3()
            finishMarker = API.createMarker(4, finishPosition, new Vector3(), rotation, new Vector3(7, 7, 10), 0, 151, 201, 170)
            API.setEntityDimension(finishMarker, dimension)
        }
        API.setEntityDimension(currentMarker, dimension)
        API.setWaypoint(position.X, position.Y)
    }

    function hideCurrentPoint(): void {
        if (currentMarker) {
            API.deleteEntity(currentMarker)
        }
        if (finishMarker) {
            API.deleteEntity(finishMarker)
        }
        API.removeWaypoint()
    }
}