/// <reference path='../../types-gt-mp/index.d.ts' />

import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'

export class CharCreator {
    private static instance: CharCreator
    private cef: Cef

    private constructor() {
        this.cef = new Cef('CharCreator', 'ui/char/index.html')
        this.cef.on('ShowView', this.setView)
        this.cef.on('ChangeSkin', this.changeSkin)
        this.cef.on('SaveChar', this.saveChar )
        ServerEventHandler.getInstance().on('ShowCharCreate', this.showCharCreator)
        ServerEventHandler.getInstance().on('HideCharCreate', this.hideCharCreator)
        ServerEventHandler.getInstance().on('NameAlreadyExist', this.nameAlreadyExist)
    }

    public static getInstance (): CharCreator {
        if (!CharCreator.instance) {
            CharCreator.instance = new CharCreator()
        }
        return CharCreator.instance
    }

    private showCharCreator() {
        CharCreator.instance.setBlendData(API.getLocalPlayer(), 0, 0, 0, 0)
        CharCreator.instance.cef.setCursorVisible(true)
        CharCreator.instance.cef.load()
        CharCreator.instance.setCamera()
    }

    private hideCharCreator() {
        CharCreator.instance.cef.destroy()
        API.setGameplayCameraActive()
    }

    private changeSkin(args: any[]) {
        let skin = args[0].Skin as number
        API.setPlayerSkin(skin)
        CharCreator.instance.setView(args)
    }

    private setView(args: any[]) {
        let char = args[0]
        let fatherFace = char.FatherFace as number
        let motherFace = char.MotherFace as number
        let skinMix = char.SkinMix as number
        let shapeMix = char.ShapeMix as number
        let hairStyle = char.HairStyle as number
        let hairColor = char.HairColor as number
        let eyesColor = char.EyesColor as number        
        CharCreator.instance.setAppearance(fatherFace, motherFace, skinMix, shapeMix, hairStyle, hairColor, eyesColor);
    }

    private setAppearance(fatherFace: number, motherFace: number, skinMix: number, shapeMix: number, hairStyle: number, hairColor: number, eyesColor: number) {
        let player = API.getLocalPlayer()
        CharCreator.instance.setBlendData(player, fatherFace, motherFace, skinMix, shapeMix);
        API.setPlayerHairStyle(player, hairStyle, hairColor, 0, '', '');
        API.setPlayerEyeColor(player, eyesColor);
    }

    private setBlendData(player: LocalHandle, fatherFace: number, motherFace: number, skinMix: number, shapeMix: number) {
        API.setPlayerHeadBlendData(player, motherFace, fatherFace, 0, motherFace, fatherFace, 0, shapeMix, skinMix, 0.0, true)
    }

    private saveChar(args: any[]) {
        let char = args[0]
        let name = args[1]
        let surname = args[2]
        API.triggerServerEvent('SaveCharacter', JSON.stringify(char), name, surname)
    }

    private nameAlreadyExist() {
        CharCreator.instance.cef.eval('nameAlreadyExist()')
    }

    private setCamera() {
        let position = new Vector3(403.06, -998.1, -98.40)
        let rotation = new Vector3(0.00, 0.00, 2.5)
        let camera = API.createCamera(position, rotation)
        API.setActiveCamera(camera)
    }
}