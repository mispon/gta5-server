import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace BusDriver { 
    let blip: LocalHandle   
    let marker: LocalHandle    

    ServerEventHandler.getInstance().on('ShowNextBusStop', showNextBusStop)
    ServerEventHandler.getInstance().on('HideBusRoute', deleteCurrentPoint)

    function showNextBusStop(args: any[]): void {
        let position = args[0] as Vector3
        deleteCurrentPoint()        
        marker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(3, 3, 3), 255, 186, 84, 255)
        blip = createBlip(position)
        API.setWaypoint(position.X, position.Y)
    }

    function deleteCurrentPoint(): void {
        if (marker) {
            API.deleteEntity(marker)
            API.deleteEntity(blip)
        }
        API.removeWaypoint()
    }

    function createBlip(position: Vector3) {        
        let result = API.createBlip(position)
        API.setBlipSprite(result, 1)
        API.setBlipColor(result, 3)
        API.setBlipName(result, 'Остановка')
        return result
    }
}