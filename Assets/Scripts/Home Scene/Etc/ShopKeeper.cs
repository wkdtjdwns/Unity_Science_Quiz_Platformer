using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KoreanTyper;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField]
    private Text shop_keeper_text;

    [SerializeField]
    private string[] purchase_texts;
    [SerializeField]
    private string[] sell_texts;
    [SerializeField]
    private string[] chat_texts;

    [SerializeField]
    private Sprite[] shop_keeper_sprites;
    Image img;

    public bool is_talk;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    public void OnTalk(string type)
    {
        StopAllCoroutines();

        is_talk = true;
        ShopKeeperTalk(type);
        StartCoroutine(KeeperAnim());
    }

    private void ShopKeeperTalk(string type)
    {
        switch (type)
        {
            case "purchase":
                int ran_purchase_index = Random.Range(0, purchase_texts.Length);
                StartCoroutine(TypingCoroutine(purchase_texts[ran_purchase_index], 0.05f));
                break;

            case "sell":
                int ran_sell_index = Random.Range(0, sell_texts.Length);
                StartCoroutine(TypingCoroutine(sell_texts[ran_sell_index], 0.05f));
                break;

            case "chat":
                int ran_chat_index = Random.Range(0, chat_texts.Length);
                StartCoroutine(TypingCoroutine(chat_texts[ran_chat_index], 0.05f));
                break;
        }
    }

    private IEnumerator TypingCoroutine(string str, float next_typing_time)
    {
        int typing_length = str.GetTypingLength();

        // Hierarchy 창에서 적은 \n(줄바꿈 표시)가 작동하도록 함
        str = str.Replace("\\n", "\n");

        for (int index = 0; index <= typing_length; index++)
        {
            SoundManager.instance.PlaySound("talk");

            shop_keeper_text.text = str.Typing(index);
            yield return new WaitForSeconds(next_typing_time);
        }

        is_talk = false;
    }

    private IEnumerator KeeperAnim()
    {
        int index = 0;

        while (is_talk)
        {
            index = index < 1 ? 1 : 0;
            
            img.sprite = shop_keeper_sprites[index];

            yield return new WaitForSeconds(0.1f);
        }

        img.sprite = shop_keeper_sprites[0];

        StopAllCoroutines();
    }
}
