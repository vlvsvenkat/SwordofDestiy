using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectionzone : MonoBehaviour
{
    public List<Collider2D> detectedcolliders = new List<Collider2D>();
    Collider2D col;
    private void Awake(){
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision){
        detectedcolliders.Add(collision);
    }
    private void OnTriggerExit2D(Collider2D collision){
        detectedcolliders.Remove(collision);
    }

}
