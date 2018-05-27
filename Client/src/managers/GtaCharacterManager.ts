import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export namespace GtaCharacterManager {
    ServerEventHandler.getInstance().on('UpdateAppearance', setAppearance)

    API.onEntityStreamIn.connect((entity, entityType) => {
        const playerType = 6
        if (entityType == playerType) {
            setPlayerAppearance(entity)
        }
    })

    function setAppearance(args: any[]) {
        let player = args[0] as LocalHandle
        setPlayerAppearance(player)
    }

    export function setPlayerAppearance(player: LocalHandle) {        
        if (!IsPlayerCorrect(player)) {
            return
        }
        setFace(player)
        setHairColor(player)
        setEyesColor(player)
    }

    function setFace(player: LocalHandle) {
        const shapeFirstId = API.getEntitySyncedData(player, "GTAO_SHAPE_FIRST_ID")
        const shapeSecondId = API.getEntitySyncedData(player, "GTAO_SHAPE_SECOND_ID")
        const skinFirstId = API.getEntitySyncedData(player, "GTAO_SKIN_FIRST_ID")
        const skinSecondId = API.getEntitySyncedData(player, "GTAO_SKIN_SECOND_ID")
        const shapeMix = API.f(API.getEntitySyncedData(player, "GTAO_SHAPE_MIX"))
        const skinMix = API.f(API.getEntitySyncedData(player, "GTAO_SKIN_MIX"))
        API.callNative("SET_PED_HEAD_BLEND_DATA", player, shapeFirstId, shapeSecondId, 0, skinFirstId, skinSecondId, 0, shapeMix, skinMix, 0, false)
    }

    function setHairColor(player: LocalHandle) {
        const hairColor = API.getEntitySyncedData(player, "GTAO_HAIR_COLOR")
        API.callNative("_SET_PED_HAIR_COLOR", player, hairColor, 0)
    }

    function setEyesColor(player: LocalHandle) {
        const eyeColor = API.getEntitySyncedData(player, "GTAO_EYE_COLOR")
        API.callNative("_SET_PED_EYE_COLOR", player, eyeColor)
    }

    function IsPlayerCorrect(player: LocalHandle) {
        return API.isPed(player) && 
               API.getEntitySyncedData(player, "GTAO_HAS_CHARACTER_DATA") === true &&
              (API.getEntityModel(player) == 1885233650 || API.getEntityModel(player) == -1667301416)    
    }
}