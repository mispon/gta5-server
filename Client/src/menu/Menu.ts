export abstract class Menu {
    protected menu: NativeUI.UIMenu
    public isOpen: boolean = false

    constructor(title: string, subtitle: string = 'Доступные действия:') {
        this.menu = API.createMenu(title, subtitle, 0, 0, 6)        
        this.fillMenuByDefault()
    }

    protected abstract fillMenuByDefault(): void

    public triggerVisible(args: any[] = []): void {  
        API.closeAllMenus()      
        this.isOpen = !this.isOpen
        this.menu.Visible = this.isOpen
    }

    public show(args: any[] = []): void {
        API.closeAllMenus()
        this.isOpen = true
        this.menu.Visible = this.isOpen
    }

    public hide(): void {
        this.isOpen = false
        this.menu.Visible = this.isOpen
    }
}