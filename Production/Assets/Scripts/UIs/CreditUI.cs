using UnityEngine;

namespace MoonBunny.UIs
{
    public class CreditUI : UI
    {
        public void OnInstagramButtonClicked()
        {
            Application.OpenURL("https://www.instagram.com/jump_have_bunny_official/");            
        }

        public void OnVoteButtonClicked()
        {
            Application.OpenURL("https://pale-flower-e0d.notion.site/5-9793ec7a499b4f9dae63af25a17b8b71");            
        }

        public void OnSJGithubClicked()
        {
            Application.OpenURL("https://github.com/dkstlzu");
        }
    }
}