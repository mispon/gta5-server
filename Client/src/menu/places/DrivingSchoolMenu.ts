import { Menu } from '../Menu'

export class DrivingSchoolMenu extends Menu {
    private isGoodPlayer: boolean

    constructor() {
        super('Автошкола', 'Сдать теорию или практику:')        
    }

    protected fillMenuByDefault(): void {
        const theoryPrice = 150
        let theoryItem = API.createMenuItem(`Сдать теорию (${theoryPrice}$)`, 'Теоретический экзамен из 8 вопросов');
        this.menu.AddItem(theoryItem)
        theoryItem.Activated.connect(() => API.triggerServerEvent('TheoryExam', theoryPrice))

        const practicePrice = 300
        let practiceItem = API.createMenuItem(`Сдать практику (${practicePrice}$)`, 'Практический экзамен езды по городу');
        this.menu.AddItem(practiceItem)
        practiceItem.Activated.connect(() => API.triggerServerEvent('PracticeExam', practicePrice))

        const bribePrice = 800
        let bribeLicenceItem = API.createMenuItem(`Купить права (${bribePrice}$)`, 'Дать взятку и получить права без экзаменов');
        this.menu.AddItem(bribeLicenceItem)
        bribeLicenceItem.Activated.connect(() => API.triggerServerEvent('BribeLicence', bribePrice))
    }
}