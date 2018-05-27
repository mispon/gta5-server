/// <reference path='../../../types-gt-mp/index.d.ts' />

import { VectorConverter } from '../../utils/VectorConverter'
import { AfkVehicle } from '../../models/AfkVehicle'
import { ServerEventHandler } from '../../event-handlers/ServerEventHandler'

namespace ParachuteManager {
    let afkVehicles: Map<number, LocalHandle> = new Map()
    let currentMarker: LocalHandle
    
    ServerEventHandler.getInstance().on('ShowAfkVehicles', showAfkVehicles)
    ServerEventHandler.getInstance().on('HideAfkVehicles', hideAfkVehicles)
    ServerEventHandler.getInstance().on('ShowDropZone', showDropZone)
    ServerEventHandler.getInstance().on('HideDropZone', hideDropZone)
    ServerEventHandler.getInstance().on('AddAfkVehicle', addAfkVehicle)
    ServerEventHandler.getInstance().on('RemoveAfkVehicle', removeAfkVehicles)

    function showAfkVehicles(args: any[]) {
        let vehicles = JSON.parse(args[0]) as AfkVehicle[]
        vehicles.forEach(vehicle => addVehicle(vehicle))
    }

    function hideAfkVehicles() {
        afkVehicles.forEach(blip => {
            API.deleteEntity(blip)
        })
        afkVehicles.clear()
    }

    function addAfkVehicle(args: any[]) {
        let vehicle = JSON.parse(args[0]) as AfkVehicle
        addVehicle(vehicle)
    }

    function removeAfkVehicles(args: any[]) {
        let vehicle = JSON.parse(args[0]) as AfkVehicle
        if (!afkVehicles.has(vehicle.Id)) {
            return
        }
        let blip = afkVehicles.get(vehicle.Id) as LocalHandle
        API.deleteEntity(blip)      
        afkVehicles.delete(vehicle.Id)
    }

    function addVehicle(vehicle: AfkVehicle) {
        let position = VectorConverter.convert(vehicle.Position)
        afkVehicles.set(vehicle.Id, createBlip(position))
    }

    function createBlip(position: Vector3): LocalHandle {
        let blip = API.createBlip(position)
        API.setBlipSprite(blip, 1)
        API.setBlipColor(blip, 1)
        API.setBlipName(blip, 'Брошенный транспорт')
        return blip
    }

    function showDropZone(args: any[]) {
        let vector = JSON.parse(args[0]) as Vector3
        let position = VectorConverter.convert(vector)
        currentMarker = API.createMarker(1, position, new Vector3(), new Vector3(), new Vector3(4, 4, 2), 16, 125, 161, 255)
        API.setWaypoint(position.X, position.Y)
    }

    function hideDropZone() {
        if (currentMarker) API.deleteEntity(currentMarker)        
        API.removeWaypoint()
    }
}