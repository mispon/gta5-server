import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace BuilderManager {
    let pointMarker: LocalHandle
    let pointBlip: LocalHandle
    let pointLabel: LocalHandle

    ServerEventHandler.getInstance().on('ShowBuilderPoint', showNextPoint)
    ServerEventHandler.getInstance().on('HideBuilderPoint', deleteCurrentPoint)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (args.KeyCode == Keys.E && API.hasEntitySyncedData(player, 'IsBuilder') && API.hasEntitySyncedData(player, 'OnBuildingTarget')) {
            API.resetEntitySyncedData(player, 'OnBuildingTarget')
            API.triggerServerEvent('ProcessBuilderPoint')
        }
    })

    function showNextPoint(args: any[]) { 
        deleteCurrentPoint()       
        let position = args[0] as Vector3
        pointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(1, 1, 1), 2, 224, 244, 150)
        createBlip(position)
        pointLabel = API.createTextLabel('Нажмите ~y~Е', position.Add(new Vector3(0, 0, 1.5)), 10, 0.5)
    }

    function deleteCurrentPoint() {
        if (!pointMarker) {
           return
        }
        API.deleteEntity(pointMarker)
        API.deleteEntity(pointBlip)
        API.deleteEntity(pointLabel)
    }

    function createBlip(position: Vector3) {        
        pointBlip = API.createBlip(position)
        API.setBlipSprite(pointBlip, 1)
        API.setBlipColor(pointBlip, 3)
        API.setBlipName(pointBlip, 'Рабочая точка')
        API.setBlipFlashing(pointBlip, false)
    }
}