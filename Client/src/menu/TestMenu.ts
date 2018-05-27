/// <reference path='../../types-gt-mp/index.d.ts' />

import { Menu } from './Menu'
import { Cef } from '../browser/Cef'

export class TestMenu extends Menu {
    private static cef: Cef

    constructor() {
        super(' ')
    }

    protected fillMenuByDefault() {
        // TestMenu.scenarios.forEach(e => {
        //     let item = API.createMenuItem(e, '')
        //     item.Activated.connect(() => API.playPlayerScenario(e))
        //     this.menu.AddItem(item)
        // })
        
        this.createBadgePreview()
    }

    private static scenarios = [
        'WORLD_HUMAN_AA_COFFEE',
        'WORLD_HUMAN_AA_SMOKE',
        'WORLD_HUMAN_BINOCULARS',
        'WORLD_HUMAN_BUM_FREEWAY',
        'WORLD_HUMAN_BUM_SLUMPED',
        'WORLD_HUMAN_BUM_STANDING',
        'WORLD_HUMAN_BUM_WASH',
        'WORLD_HUMAN_CAR_PARK_ATTENDANT',
        'WORLD_HUMAN_CHEERING',
        'WORLD_HUMAN_CLIPBOARD',
        'WORLD_HUMAN_CONST_DRILL',
        'WORLD_HUMAN_COP_IDLES',
        'WORLD_HUMAN_DRINKING',
        'WORLD_HUMAN_DRUG_DEALER',
        'WORLD_HUMAN_DRUG_DEALER_HARD',
        'WORLD_HUMAN_MOBILE_FILM_SHOCKING',
        'WORLD_HUMAN_GARDENER_LEAF_BLOWER',
        'WORLD_HUMAN_GARDENER_PLANT',
        'WORLD_HUMAN_GOLF_PLAYER',
        'WORLD_HUMAN_GUARD_PATROL',
        'WORLD_HUMAN_GUARD_STAND',
        'WORLD_HUMAN_GUARD_STAND_ARMY',
        'WORLD_HUMAN_HAMMERING',
        'WORLD_HUMAN_HANG_OUT_STREET',
        'WORLD_HUMAN_HIKER_STANDING',
        'WORLD_HUMAN_HUMAN_STATUE',
        'WORLD_HUMAN_JANITOR',
        'WORLD_HUMAN_JOG_STANDING',
        'WORLD_HUMAN_LEANING',
        'WORLD_HUMAN_MAID_CLEAN',
        'WORLD_HUMAN_MUSCLE_FLEX',
        'WORLD_HUMAN_MUSCLE_FREE_WEIGHTS',
        'WORLD_HUMAN_MUSICIAN',
        'WORLD_HUMAN_PAPARAZZI',
        'WORLD_HUMAN_PARTYING',
        'WORLD_HUMAN_PICNIC',
        'WORLD_HUMAN_PROSTITUTE_HIGH_CLASS',
        'WORLD_HUMAN_PROSTITUTE_LOW_CLASS',
        'WORLD_HUMAN_PUSH_UPS',
        'WORLD_HUMAN_SEAT_LEDGE',
        'WORLD_HUMAN_SEAT_LEDGE_EATING',
        'WORLD_HUMAN_SEAT_STEPS',
        'WORLD_HUMAN_SEAT_WALL',
        'WORLD_HUMAN_SEAT_WALL_EATING',
        'WORLD_HUMAN_SEAT_WALL_TABLET',
        'WORLD_HUMAN_SECURITY_SHINE_TORCH',
        'WORLD_HUMAN_SIT_UPS',
        'WORLD_HUMAN_SMOKING',
        'WORLD_HUMAN_SMOKING_POT',
        'WORLD_HUMAN_STAND_FIRE',
        'WORLD_HUMAN_STAND_FISHING',
        'WORLD_HUMAN_STAND_IMPATIENT',
        'WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT',
        'WORLD_HUMAN_STAND_MOBILE',
        'WORLD_HUMAN_STAND_MOBILE_UPRIGHT',
        'WORLD_HUMAN_STRIP_WATCH_STAND',
        'WORLD_HUMAN_STUPOR',
        'WORLD_HUMAN_SUNBATHE',
        'WORLD_HUMAN_SUNBATHE_BACK',
        'WORLD_HUMAN_SUPERHERO',
        'WORLD_HUMAN_SWIMMING',
        'WORLD_HUMAN_TENNIS_PLAYER',
        'WORLD_HUMAN_TOURIST_MAP',
        'WORLD_HUMAN_TOURIST_MOBILE',
        'WORLD_HUMAN_VEHICLE_MECHANIC',
        'WORLD_HUMAN_WELDING',
        'WORLD_HUMAN_WINDOW_SHOP_BROWSE',
        'WORLD_HUMAN_YOGA'
    ]

    private createBadgePreview() {
        let item0 = API.createMenuItem(`Item 0`, '')
        item0.SetLeftBadge(BadgeStyle.None)
        this.menu.AddItem(item0)

        let item1 = API.createMenuItem(`Item 1`, '')
        item1.SetLeftBadge(BadgeStyle.BronzeMedal)
        this.menu.AddItem(item1)

        let item2 = API.createMenuItem(`Item 2`, '')
        item2.SetLeftBadge(BadgeStyle.GoldMedal)
        this.menu.AddItem(item2)

        let item3 = API.createMenuItem(`Item 3`, '')
        item3.SetLeftBadge(BadgeStyle.SilverMedal)
        this.menu.AddItem(item3)

        let item4 = API.createMenuItem(`Item 4`, '')
        item4.SetLeftBadge(BadgeStyle.Alert)
        this.menu.AddItem(item4)

        let item5 = API.createMenuItem(`Item 5`, '')
        item5.SetLeftBadge(BadgeStyle.Crown)
        this.menu.AddItem(item5)

        let item6 = API.createMenuItem(`Item 6`, '')
        item6.SetLeftBadge(BadgeStyle.Ammo)
        this.menu.AddItem(item6)

        let item7 = API.createMenuItem(`Item 7`, '')
        item7.SetLeftBadge(BadgeStyle.Armour)
        this.menu.AddItem(item7)

        let item8 = API.createMenuItem(`Item 8`, '')
        item8.SetLeftBadge(BadgeStyle.Barber)
        this.menu.AddItem(item8)

        let item9 = API.createMenuItem(`Item 9`, '')
        item9.SetLeftBadge(BadgeStyle.Clothes)
        this.menu.AddItem(item9)

        let item10 = API.createMenuItem(`Item 10`, '')
        item10.SetLeftBadge(BadgeStyle.Franklin)
        this.menu.AddItem(item10)

        let item11 = API.createMenuItem(`Item 11`, '')
        item11.SetLeftBadge(BadgeStyle.Bike)
        this.menu.AddItem(item11)

        let item12 = API.createMenuItem(`Item 12`, '')
        item12.SetLeftBadge(BadgeStyle.Car)
        this.menu.AddItem(item12)

        let item13 = API.createMenuItem(`Item 13`, '')
        item13.SetLeftBadge(BadgeStyle.Gun)
        this.menu.AddItem(item13)

        let item14 = API.createMenuItem(`Item 14`, '')
        item14.SetLeftBadge(BadgeStyle.Heart)
        this.menu.AddItem(item14)

        let item15 = API.createMenuItem(`Item 15`, '')
        item15.SetLeftBadge(BadgeStyle.Makeup)
        this.menu.AddItem(item15)

        let item16 = API.createMenuItem(`Item 16`, '')
        item16.SetLeftBadge(BadgeStyle.Mask)
        this.menu.AddItem(item16)

        let item17 = API.createMenuItem(`Item 17`, '')
        item17.SetLeftBadge(BadgeStyle.Michael)
        this.menu.AddItem(item17)

        let item18 = API.createMenuItem(`Item 18`, '')
        item18.SetLeftBadge(BadgeStyle.Star)
        this.menu.AddItem(item18)

        let item19 = API.createMenuItem(`Item 19`, '')
        item19.SetLeftBadge(BadgeStyle.Tatoo)
        this.menu.AddItem(item19)

        let item20 = API.createMenuItem(`Item 20`, '')
        item20.SetLeftBadge(BadgeStyle.Trevor)
        this.menu.AddItem(item20)

        let item21 = API.createMenuItem(`Item 21`, '')
        item21.SetLeftBadge(BadgeStyle.Lock)
        this.menu.AddItem(item21)

        let item22 = API.createMenuItem(`Item 22`, '')
        item22.SetLeftBadge(BadgeStyle.Tick)
        this.menu.AddItem(item22)

        let item23 = API.createMenuItem(`Item 23`, '')
        item23.SetLeftBadge(BadgeStyle.Sale)
        this.menu.AddItem(item23)
    }
}