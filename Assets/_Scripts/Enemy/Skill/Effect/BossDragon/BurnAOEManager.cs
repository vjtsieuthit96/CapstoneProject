using UnityEngine;

public class BurnAOEManager : MonoBehaviour
{  
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {          
            NegativeEffect player = other.GetComponent<NegativeEffect>();
            CharacterConfigurator character = other.GetComponent<CharacterConfigurator>();
            if (player != null)
            {               
                player.ApplyBurn(character.PlayerMaxHealth * 0.05f, 10f);
               
            }
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.CompareTag("Player"))
    //    {
    //        Debug.Log("Player entered Burn AOE");
    //        NegativeEffect player = other.GetComponent<NegativeEffect>();
    //        CharacterConfigurator character = other.GetComponent<CharacterConfigurator>();
    //        if (player != null)
    //        {
    //            player.ApplyBurn(character.PlayerMaxHealth * 0.05f, 10f);
    //        }
    //    }
    //}

}
