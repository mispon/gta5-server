const addCloseItem = (menu: NativeUI.UIMenu, name: string = 'Назад') => {
    let backItem = API.createMenuItem(`~r~${name}`, '')
    backItem.Activated.connect(() => menu.GoBack())       
    menu.AddItem(backItem)
}

export default addCloseItem