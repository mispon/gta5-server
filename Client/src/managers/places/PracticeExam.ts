import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace PracticeExam { 
    let currentMarker: LocalHandle
    const MAX_DAMAGE_COUNT: number = 3
    let damageCount: number = 0

    ServerEventHandler.getInstance().on('ShowNextDrivingExamPoint', showNextPoint)
    ServerEventHandler.getInstance().on('FinishPracticeExam', finishExam)

    API.onVehicleHealthChange.connect((oldValue: number) => {
        let player = API.getLocalPlayer()
        let vehicle = API.getPlayerVehicle(player)
        let isSchoolCar = API.getEntitySyncedData(vehicle, "SchoolCar") as boolean        
        if (!isSchoolCar) {
            return
        }
        damageCount++
        API.sendNotification(`~r~Повреждений ${damageCount} / ${MAX_DAMAGE_COUNT}`)
        if (damageCount >= MAX_DAMAGE_COUNT) {
            API.triggerServerEvent('DamageCountExceeded')
        }
    })

    function showNextPoint(args: any[]): void {
        let position = args[0] as Vector3
        deleteCurrentPoint()
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(3, 3, 15), 218, 0, 234, 150)
        API.setWaypoint(position.X, position.Y)
    }

    function finishExam(args: any[]): void {
        deleteCurrentPoint()
        API.removeWaypoint()
        damageCount = 0
    }

    function deleteCurrentPoint(): void {
        if (currentMarker) {
            API.deleteEntity(currentMarker)
        }
    }
}