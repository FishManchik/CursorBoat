using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class ClickPlayerPosition : MonoBehaviour
{
    [SerializeField] private Transform player;
    private IInputHandler input;

    // --- Windows API константы ---
    const int GWL_EXSTYLE = -20;
    const int WS_EX_TRANSPARENT = 0x20;
    const int WS_EX_LAYERED = 0x80000;

    const uint MOUSEEVENTF_LEFTDOWN = 0x02;
    const uint MOUSEEVENTF_LEFTUP = 0x04;

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    // --- WinAPI ---
    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll")]
    static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

    private IntPtr hwnd;

    private void Start()
    {
        // Получаем хэндл окна игры один раз
        hwnd = GetActiveWindow();
    }

    private void Update()
    {
        if (!player)
        {
            Debug.LogError("А ГДЕ МОЙ PLAYER?!");
            return;
        }

        // Нажатие пробела — клик и click-through
        if (input.IsSpacePressed)
        {
            EnableClickThrough();
            ClickPlayer();
        }
        else
        {
            // Отпустил пробел — возвращаем фокус на окно Unity
            RestoreWindowFocus();
            DisableClickThrough();
        }
    }

    /// <summary>
    /// Клик по позиции игрока на экране
    /// </summary>
    private void ClickPlayer()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position);

        int x = (int)screenPos.x;
        int y = Screen.height - (int)screenPos.y; // Unity y→Windows y

        SetCursorPos(x, y);
        mouse_event(MOUSEEVENTF_LEFTDOWN, (uint)x, (uint)y, 0, 0);
        mouse_event(MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
    }

    /// <summary>
    /// Включает click-through для окна, чтобы клик проходил "сквозь"
    /// </summary>
    private void EnableClickThrough()
    {
        int style = GetWindowLong(hwnd, GWL_EXSTYLE);
        SetWindowLong(hwnd, GWL_EXSTYLE, style | WS_EX_TRANSPARENT | WS_EX_LAYERED);
    }

    /// <summary>
    /// Выключает click-through, чтобы окно снова принимало клики
    /// </summary>
    private void DisableClickThrough()
    {
        int style = GetWindowLong(hwnd, GWL_EXSTYLE);
        style &= ~(WS_EX_TRANSPARENT | WS_EX_LAYERED);
        SetWindowLong(hwnd, GWL_EXSTYLE, style);
    }

    /// <summary>
    /// Возвращает фокус на окно Unity
    /// </summary>
    private void RestoreWindowFocus()
    {
        SetForegroundWindow(hwnd);
    }
}

//CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT CHAT GPT 

/* ⠀⠀⠀⠀⠀⢀⣤⠤⠤⠤⠤⠤⠤⠤⠤⠤⠤⢤⣤⣀⣀⡀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⢀⡼⠋⠀⣀⠄⡂⠍⣀⣒⣒⠂⠀⠬⠤⠤⠬⠍⠉⠝⠲⣄⡀⠀⠀
⠀⠀⠀⢀⡾⠁⠀⠊⢔⠕⠈⣀⣀⡀⠈⠆⠀⠀⠀⡍⠁⠀⠁⢂⠀⠈⣷⠀⠀
⠀⠀⣠⣾⠥⠀⠀⣠⢠⣞⣿⣿⣿⣉⠳⣄⠀⠀⣀⣤⣶⣶⣶⡄⠀⠀⣘⢦⡀
⢀⡞⡍⣠⠞⢋⡛⠶⠤⣤⠴⠚⠀⠈⠙⠁⠀⠀⢹⡏⠁⠀⣀⣠⠤⢤⡕⠱⣷
⠘⡇⠇⣯⠤⢾⡙⠲⢤⣀⡀⠤⠀⢲⡖⣂⣀⠀⠀⢙⣶⣄⠈⠉⣸⡄⠠⣠⡿
⠀⠹⣜⡪⠀⠈⢷⣦⣬⣏⠉⠛⠲⣮⣧⣁⣀⣀⠶⠞⢁⣀⣨⢶⢿⣧⠉⡼⠁
⠀⠀⠈⢷⡀⠀⠀⠳⣌⡟⠻⠷⣶⣧⣀⣀⣹⣉⣉⣿⣉⣉⣇⣼⣾⣿⠀⡇⠀
⠀⠀⠀⠈⢳⡄⠀⠀⠘⠳⣄⡀⡼⠈⠉⠛⡿⠿⠿⡿⠿⣿⢿⣿⣿⡇⠀⡇⠀
⠀⠀⠀⠀⠀⠙⢦⣕⠠⣒⠌⡙⠓⠶⠤⣤⣧⣀⣸⣇⣴⣧⠾⠾⠋⠀⠀⡇⠀
⠀⠀⠀⠀⠀⠀⠀⠈⠙⠶⣭⣒⠩⠖⢠⣤⠄⠀⠀⠀⠀⠀⠠⠔⠁⡰⠀⣧⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠲⢤⣀⣀⠉⠉⠀⠀⠀⠀⠀⠁⠀⣠⠏⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⠉⠛⠒⠲⠶⠤⠴⠒⠚⠁
*/