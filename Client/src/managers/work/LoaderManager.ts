import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace LoaderManager {
    let takeMarker: LocalHandle
    let takeBlip: LocalHandle
    let putMarker: LocalHandle
    let putBlip: LocalHandle

    ServerEventHandler.getInstance().on('ShowTakeLoaderPoint', showTakePoint)
    ServerEventHandler.getInstance().on('ShowPutLoaderPoint', showPutPoint)
    ServerEventHandler.getInstance().on('HideLoaderPoints', hidePoints)

    // показать точку выдачи
    function showTakePoint(args: any[]) {        
        let position = args[0] as Vector3
        deleteTakePoint()
        takeMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(2, 2, 2), 2, 224, 244, 150)
        createBlip(position, 'take')
    }

    // показать точку сдачи
    function showPutPoint(args: any[]) {
        let position = args[0] as Vector3
        deletePutPoint()
        putMarker = API.createMarker(29, position, new Vector3(), new Vector3(), new Vector3(3, 3, 3), 25, 255, 0, 150)
        createBlip(position, 'put')
    }

    // скрыть все точки
    function hidePoints() {
        deleteTakePoint()
        deletePutPoint()
    }

    // удаить точку получения
    function deleteTakePoint() {
        if (takeMarker) {
            API.deleteEntity(takeMarker)
        }
        if (takeBlip) {
            API.deleteEntity(takeBlip)
        }
    }

    // удалить точку сдачи
    function deletePutPoint() {
        if (putMarker) {
            API.deleteEntity(putMarker)
        }
        if (putBlip) {
            API.deleteEntity(putBlip)
        }
    }

    // создать отметку на радаре
    function createBlip(position: Vector3, type: string) {        
        switch (type) {
            case 'take':
                takeBlip = API.createBlip(position)
                API.setBlipSprite(takeBlip, 1)
                API.setBlipColor(takeBlip, 84)
                API.setBlipName(takeBlip, 'Получение груза')
                break;
            case 'put':
                putBlip = API.createBlip(position)
                API.setBlipSprite(putBlip, 1)
                API.setBlipColor(putBlip, 69)
                API.setBlipName(putBlip, 'Сдача груза')
                break;
        }        
    }
}