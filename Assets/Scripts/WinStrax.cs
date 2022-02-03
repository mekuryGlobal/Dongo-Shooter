using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using NBitcoin;
using Stratis.Bitcoin.Networks;
using Unity3dApi;
using UnityEngine;
using UnityEngine.UI;

public class WinStrax : MonoBehaviour
{
    
    public Button InitializeApi_Button, CopyAddress_Button, RefreshBalance_Button, SendTx_Button;
    public Button PopupPanelOk_Button;

    private StratisUnityManager stratisUnityManager;
    private string Mnemonic, MainAddress;
    public GameObject PopupPanel;
    public Text PlayerAddress_Text, Balance_Text;
    public Text PopupPanel_Text;
    public InputField PlayerMnemonic;
    private string ApiUrl = "http://localhost:44336/";
    


    // Start is called before the first frame update
    void Start()
    {
        PopupPanel.SetActive(false);

       
        InitializeApi_Button.onClick.AddListener(InitializeApi_ButtonCall);
        CopyAddress_Button.onClick.AddListener(CopyAddress_ButtonCall);
        RefreshBalance_Button.onClick.AddListener(RefreshBalance_ButtonCall);
        SendTx_Button.onClick.AddListener(() => this.StartCoroutine(SendTx_ButtonCall()));
        PopupPanelOk_Button.onClick.AddListener(() => PopupPanel.SetActive(false));

        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitializeApi_ButtonCall()
    {
        Unity3dClient client = new Unity3dClient(ApiUrl);
        Mnemonic = Mnemonic = PlayerMnemonic.text;
        //"upset blame piano few merit purpose dignity sort patient angry segment student";
       
        try
        {
            stratisUnityManager = new StratisUnityManager(client, new StraxMain(), Mnemonic);
            PlayerAddress_Text.text = stratisUnityManager.GetAddress().ToString();
            StartCoroutine(RefreshBalance());
        }
        catch (Exception e)
        {
            DisplayPopup(e.ToString());
        }
    }

    private IEnumerator RefreshBalance()
    {
        decimal balance = -1;

        Task task = Task.Run(async () =>
        {
            try
            {
                balance = await stratisUnityManager.GetBalanceAsync();
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                return;
            }
        });

        while (!task.IsCompleted)
            yield return null;

        Balance_Text.text = balance.ToString();
    }

    private void CopyAddress_ButtonCall()
    {
        GUIUtility.systemCopyBuffer = PlayerAddress_Text.text;
    }

    private void RefreshBalance_ButtonCall()
    {
        this.StartCoroutine(RefreshBalance());
    }

    private IEnumerator SendTx_ButtonCall()
    {
        Unity3dClient client = new Unity3dClient(ApiUrl);
        Mnemonic = "leopard fire legal door attract stove similar response photo prize seminar frown";
        StratisUnityManager stratisUnityManager2 = new StratisUnityManager(client, new StraxMain(), Mnemonic);
        string destAddress = PlayerAddress_Text.text;
        Money amount = new Money(Decimal.Parse("20"), MoneyUnit.BTC);

        string txHash = null;
        string error = null;

        Task task = Task.Run(async () =>
        {
            try
            {
                txHash = await stratisUnityManager2.SendTransactionAsync(destAddress, amount);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                error = e.ToString();
                return;
            }
        });

        while (!task.IsCompleted)
            yield return null;

        if (error != null)
        {
            this.DisplayPopup("Error sending tx: " + error);
        }
        else
        {
            this.DisplayPopup(string.Format("Transaction {0} to {1} with amount: {2} was sent.", txHash, destAddress, amount));
        }

        PlayerAddress_Text.text = "";


    }

    private void DisplayPopup(string text)
    {
        SynchronizationContext.Current.Post(state =>
        {
            PopupPanel.SetActive(true);
            this.PopupPanel_Text.text = text;
        }, null);
    }
}
