using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace MySampleEx
{
    /// <summary>
    /// ��ȭâ ���� Ŭ���� 
    /// ��ȭ ������ �б�
    /// ��ȭ ������ UI ����
    /// </summary>
    public class DialogUI : MonoBehaviour
    {
        #region Variables
        //XML
        public string xmlFile = "Dialog/Dialog";        // Path
        private XmlNodeList allNodes;

        private Queue<Dialog> dialogs;

        public TextMeshProUGUI nameText;
        public TextMeshProUGUI sentenceText;
        public GameObject npcImage;
        public GameObject nextButton;
        public GameObject button0;
        public GameObject button1;
        public GameObject button2;
        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // Xml ������ ���� �б�
            LoadDialogXml(xmlFile);
            nextButton.GetComponent<Button>().onClick.AddListener(DrawNextDialog);
            button0.GetComponent<Button>().onClick.AddListener(() => StartDialog(0));
            button1.GetComponent<Button>().onClick.AddListener(() => StartDialog(1));
            button2.GetComponent<Button>().onClick.AddListener(() => StartDialog(2));

            dialogs = new Queue<Dialog>();
            InitDialog();
            StartDialog(1);
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void InitDialog()
        {
            dialogs.Clear();
            npcImage.SetActive(false);
            nameText.text = "";
            sentenceText.text = "";
            nextButton.SetActive(false);
        }
        private void LoadDialogXml(string path)
        {
            TextAsset xmlFile = Resources.Load<TextAsset>(path);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);
            allNodes = xmlDoc.SelectNodes("root/Dialog");
        }

        // xml ������ �о����
        public void StartDialog(int dialogIndex)
        {
            foreach (XmlNode node in allNodes)
            {
                int num = int.Parse(node["number"].InnerText);
                if (num == dialogIndex)
                {
                    Dialog dialog = new Dialog();
                    dialog.number = num;
                    dialog.character = int.Parse(node["character"].InnerText);
                    dialog.name = node["name"].InnerText;
                    dialog.sentence = node["sentence"].InnerText;

                    dialogs.Enqueue(dialog);
                }
            }
            // ù��° ��ȭ�� �����ش�
            DrawNextDialog();
        }

        // ���� ��ȭ�� �����ش� - (ť)Dialog���� �ϳ� ������ �����ش�
        public void DrawNextDialog()
        {
            // dialogs üũ
            if (dialogs.Count == 0)
            {
                EndDialog();
                return;
            }
            StopAllCoroutines();
            // dialogs ���� �ϳ��� �����´�
            Dialog dialog = dialogs.Dequeue();

            if (dialog.character > 0)
            {
                npcImage.SetActive(true);
                npcImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Dialog/Npc/npc0" + dialog.character.ToString());
            }
            else
            {
                npcImage.SetActive(false);
            }
            nextButton.SetActive(false);
            nameText.text = dialog.name;
            StartCoroutine(typingSentence(dialog.sentence));
        }

        // �ؽ�Ʈ Ÿ�̹� ����
        IEnumerator typingSentence(string typingText)
        {
            sentenceText.text = "";
            foreach (char latter in typingText)
            {
                sentenceText.text += latter;
                yield return new WaitForSeconds(0.03f);
            }

            nextButton.SetActive(true);
        }

        // ��ȭ ����
        private void EndDialog()
        {
            InitDialog();

            // ��ȭ ����� �̺�Ʈ ó��
            // ...
        }
    }
}
