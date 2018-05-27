import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace ClanQuestManager {
    let targetBlip: LocalHandle
    let targetMarker: LocalHandle
    let targetLabel: LocalHandle
    let endPointBlip: LocalHandle
    let endPointMarker: LocalHandle

    ServerEventHandler.getInstance().on('ShowClanQuestTarget', showTarget)
    ServerEventHandler.getInstance().on('ShowClanQuestEndPoint', showEndPoint)
    ServerEventHandler.getInstance().on('HideClanQuestPoints', hidePoints)

    API.onKeyDown.connect((sender, args) => {
        let player = API.getLocalPlayer()
        if (args.KeyCode == Keys.E) {
            if (API.hasEntitySyncedData(player, 'OnRacketPoint')) {
                API.resetEntitySyncedData(player, 'OnRacketPoint')
                API.triggerServerEvent('FinishRacketPoint')
            }
            if (API.hasEntitySyncedData(player, 'OnDrugDeliveryPoint')) {
                API.resetEntitySyncedData(player, 'OnDrugDeliveryPoint')
                API.triggerServerEvent('FinishDrugDeliveryPoint')
            }
            if (API.hasEntitySyncedData(player, 'InPoliceWardrobe')) {
                API.resetEntitySyncedData(player, 'InPoliceWardrobe')
                API.triggerServerEvent('ChangeIntoPolice')
            }
            if (API.hasEntitySyncedData(player, 'InPoliceRepository')) {
                API.resetEntitySyncedData(player, 'InPoliceRepository')
                API.triggerServerEvent('FinishCaseFalsification')
            }
        }        
    })

    function showTarget(args: any[]) {
        hidePoints()
        let position = args[0] as Vector3
        let withLabel = args[1] as boolean
        targetBlip = createBlip(position, 2, 'Задание банды')
        if (withLabel) {
            targetMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(1, 1, 0.7), 114, 250, 97, 150)
            targetLabel = API.createTextLabel('Нажмите ~y~Е', position.Add(new Vector3(0, 0, 1.5)), 10, 0.5)
        }
    }

    function showEndPoint(args: any[]) {
        hidePoints()
        let position = args[0] as Vector3
        endPointBlip = createBlip(position, 3, 'Завершение задания банды')
        endPointMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(4, 4, 1.5), 0, 137, 216, 150)
    }

    function hidePoints() {
        if (targetBlip) API.deleteEntity(targetBlip)
        if (targetMarker) API.deleteEntity(targetMarker)
        if (targetLabel) API.deleteEntity(targetLabel)
        if (endPointBlip) API.deleteEntity(endPointBlip)
        if (endPointMarker) API.deleteEntity(endPointMarker)
    }

    function createBlip(position: Vector3, color: number, name: string) {        
        let result = API.createBlip(position)
        API.setBlipSprite(result, 570)
        API.setBlipColor(result, color)
        API.setBlipName(result, name)
        return result
    }
}