using ReserveBiteee.Models;

namespace ReserveBiteee.Service
{
    public interface IMenuService
    {
        int AddMenu(MenuModel menu);
        List<MenuModel> GetMenus();
        int UpdateMenu(MenuModel menu);
        int DeleteMenu(int id);

    }
}
