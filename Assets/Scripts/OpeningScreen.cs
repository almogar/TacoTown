using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public Button playbutton;
    public Animator anim;
    private string ModeGame;
    private string CameraPos;
    public Button CameraTop;
    public Button CameraFront;
    public Button Easy;
    public Button Medium;
    public Button Hard;
    public ColorBlock gray;
    public ColorBlock blue;

    void Start()
    {
        CameraPos = PlayerPrefs.GetString("CameraPos", "0");
        ModeGame = PlayerPrefs.GetString("ModeGame", "1");
        gray = Medium.GetComponent<Button>().colors;
        blue = Medium.GetComponent<Button>().colors;
        gray.normalColor = Color.gray;
        blue.normalColor = Color.cyan;
        if (!ModeGame.Equals("0"))
            Easy.colors = gray;
        if (!ModeGame.Equals("1"))
            Medium.colors = gray;
        if (!ModeGame.Equals("2"))
            Hard.colors = gray;
        if (!CameraPos.Equals("1"))
            CameraTop.colors = gray;
        if (!CameraPos.Equals("0"))
            CameraFront.colors = gray;
    }

    public void easy()
    {
        ModeGame = "0";
        Easy.colors = blue;
        Medium.colors = gray;
        Hard.colors = gray;
    }
    public void medium()
    {
        ModeGame = "1";
        Easy.colors = gray;
        Medium.colors = blue;
        Hard.colors = gray;
    }
    public void hard()
    {
        ModeGame = "2";
        Easy.colors = gray;
        Medium.colors = gray;
        Hard.colors = blue;
    }
    public void top()
    {
        CameraPos = "1";
        CameraFront.colors = gray;
        CameraTop.colors = blue;
    }
    public void front()
    {
        CameraPos = "0";
        CameraFront.colors = blue;
        CameraTop.colors = gray;
    }

    public void playgame()
    {
        SceneManager.LoadScene("Game");
    }

    public void MidToRight()
    {
        anim.Play("MenuMidToMenuRight");
    }

    public void MidToLeft()
    {
        anim.Play("MenuMidToMenuLeft");
    }

    public void LeftToMid()
    {
        //CameraPos = CameraSetting.text;
        //ModeGame = GameMode.text;
        PlayerPrefs.SetString("CameraPos", CameraPos);
        PlayerPrefs.SetString("ModeGame", ModeGame);
        anim.Play("Menu_LeftMenuToMid");
    }

    public void RightToMid()
    {
        anim.Play("Menu_RightMenuToMid");
    }

    public void QuitApp()
    {
        Application.Quit();
    }


}
