import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace TractorDriverManager {
    let pointMarker: LocalHandle
    let pointBlip: LocalHandle
    let loadedTrailerBlip: LocalHandle
    let harvestDeliveryMarker: LocalHandle
    let harvestDeliveryBlip: LocalHandle    

    ServerEventHandler.getInstance().on('ShowTractorPoint', showNextPoint)
    ServerEventHandler.getInstance().on('HideTractorPoint', deleteCurrentPoint)
    ServerEventHandler.getInstance().on('ShowLoadedTrailer', showLoadedTrailer)
    ServerEventHandler.getInstance().on('HideLoadedTrailer', hideLoadedTrailer)
    ServerEventHandler.getInstance().on('ShowHarvestDeliveryPoint', showDeliveryPoint)
    ServerEventHandler.getInstance().on('HideHarvestDeliveryPoint', deleteDeliveryPoint)

    function showNextPoint(args: any[]) { 
        deleteCurrentPoint()
        let position = args[0] as Vector3
        pointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(4, 4, 1), 112, 249, 165, 150)
        pointBlip = createBlip(position, 69, 'Рабочая точка')
    }

    function deleteCurrentPoint() {
        if (!pointMarker) {
           return
        }
        API.deleteEntity(pointMarker)
        API.deleteEntity(pointBlip)
    }

    function showLoadedTrailer(args: any[]) {
        let position = args[0] as Vector3
        loadedTrailerBlip = createBlip(position, 58, 'Загруженный прицеп', 479)
        API.setBlipFlashing(loadedTrailerBlip, true)
    }

    function hideLoadedTrailer() {
        if (loadedTrailerBlip) {
            API.deleteEntity(loadedTrailerBlip)
        }
    }

    function showDeliveryPoint(args: any[]) {
        let position = args[0] as Vector3
        harvestDeliveryMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(4, 4, 1), 0, 137, 216, 150)
        harvestDeliveryBlip = createBlip(position, 58, 'Сдача урожая')
    }

    function deleteDeliveryPoint() {
        if (!harvestDeliveryMarker) {
           return
        }
        API.deleteEntity(harvestDeliveryMarker)
        API.deleteEntity(harvestDeliveryBlip)
    }

    function createBlip(position: Vector3, color: number, name: string, sprite: number = 1) {        
        let result = API.createBlip(position)
        API.setBlipSprite(result, sprite)
        API.setBlipColor(result, color)
        API.setBlipName(result, name)
        return result
    }
}