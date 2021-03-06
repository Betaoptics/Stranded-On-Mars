using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public GameObject Bullet;
    public GameObject BulletSpawn;
    Rigidbody BulletRB;
    public int BulletSpeed;
    public float GunCoolDown;
    public float ReloadTime;
    int Magazine;
    float Timer;
    AudioSource GunAudio;
    PlayerStats PlayerStats;
    public GameObject AmmoTextObject;
    Text AmmoText;
    bool Reloading = false;
    float ReloadTimer;
    public GameObject ReloadingTextObject;
    Text ReloadingText;

    void Awake() {
        GunAudio = GetComponent<AudioSource>();
        PlayerStats = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerStats>();
        AmmoText = AmmoTextObject.GetComponent<Text>();
        ReloadingText = ReloadingTextObject.GetComponent<Text>();

        // Initialize Magazine
        Magazine = PlayerStats.MagazineSize;

        // Initialize AmmoText
        AmmoText.text = $"({Magazine.ToString()}) {PlayerStats.Ammo.ToString()}";
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(Input.GetButtonDown("Fire1") && Timer >= GunCoolDown && !PlayerStats.IsDead && Magazine != 0 && !Reloading) {
            // Play Gun Shot Audio
            GunAudio.Play();

            // Spawn Bullet
            GameObject CurrentBullet = GameObject.Instantiate(Bullet, BulletSpawn.transform.position, BulletSpawn.transform.rotation);

            // Get Bullet Rigidbody
            BulletRB = CurrentBullet.GetComponent<Rigidbody>();
            
            // Shoot Bullet
            BulletRB.velocity = -CurrentBullet.transform.up * BulletSpeed;

            // Reset Timer
            Timer = 0;

            // Use Ammo
            Magazine -= 1;

            // Update AmmoText
            AmmoText.text = $"({Magazine.ToString()}) {PlayerStats.Ammo.ToString()}";
        }

        // Tip to reload text
        if(Magazine == 0 && !Reloading) {
            ReloadingTextObject.SetActive(true);
            ReloadingText.text = "Reload using R (X)-button";
        }

        // Reload Weapon
        if(Input.GetButtonDown("Reload") && !Reloading) {
            Reloading = true;
        }

        // Reload
        if(Reloading) {
            ReloadingTextObject.SetActive(true);
            ReloadingText.text = "Reloading...";
            ReloadTimer += Time.deltaTime;
            if(PlayerStats.Ammo > 0 && Magazine < PlayerStats.MagazineSize) {
                PlayerStats.Ammo -= 1;
                Magazine += 1;
            }
            if(ReloadTimer >= ReloadTime) {
                ReloadTimer = 0;
                Reloading = false;
                // Update AmmoText
                AmmoText.text = $"({Magazine.ToString()}) {PlayerStats.Ammo.ToString()}";
                // Clear ActionText
                ReloadingText.text = "";
                ReloadingTextObject.SetActive(false);
            }
        }
    }

    public void HideGun() {
        // Disable Reloading Text
        ReloadingTextObject.SetActive(false);
    }
}
