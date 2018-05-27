import { Cef } from '../browser/Cef'
import { ServerEventHandler } from '../event-handlers/ServerEventHandler'
import { DrivingSchoolMenu } from '../menu/places/DrivingSchoolMenu'

namespace DrivingSchool {
    let cef: Cef
    let menu: DrivingSchoolMenu = new DrivingSchoolMenu()
   
    ServerEventHandler.getInstance().on('ShowAndreasMenu', showMenu)
    ServerEventHandler.getInstance().on('HideAndreasMenu', hideMenu)
    ServerEventHandler.getInstance().on('ShowTheoryExam', showTheoryExam)

    function showMenu() {
        menu.show()
    }

    function hideMenu() {
        menu.hide()
    }

    function showTheoryExam() {
        cef = new Cef('TheoryExamBrowser', 'ui/theory-exam/index.html')
        cef.setCursorVisible(true)
        cef.load()
        cef.on('finishExam', (args: any[]) => {
            finishTheoryExam(args)
        })
    }

    function finishTheoryExam(args: any[]) {
        if (args) {
            API.triggerServerEvent('FinishTheoryExam', args)
        }        
        cef.destroy()
    }
}