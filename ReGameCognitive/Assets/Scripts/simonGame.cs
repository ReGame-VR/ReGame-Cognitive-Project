using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class simonGame : MonoBehaviour
{
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject cube4;
    public GameObject cube5;

    public Material cube1color;
    public Material cube1beep;
    public Material cube2color;
    public Material cube2beep;
    public Material cube3color;
    public Material cube3beep;
    public Material cube4color;
    public Material cube4beep;
    public Material cube5color;
    public Material cube5beep;

    public int kaka;
    public int current;
    public int[] seq;
    public int i = 0;
    public int z = 0;
    public int score = 0;
    public int a = 0;
    public int round = 1;
    public int numberOfCubes;
    public float timeBetweenBeep;
    public float timeCubeLit;

    private AudioSource beep1;
    private AudioSource beep2;
    private AudioSource beep3;
    private AudioSource beep4;
    private AudioSource beep5;

    public int pushed = 0;
    public Text number;
    public Text gameover;

    // Start is called before the first frame update
    void Start()
    {
        beep1 = cube1.GetComponent<AudioSource>();
        beep2 = cube2.GetComponent<AudioSource>();
        beep3 = cube3.GetComponent<AudioSource>();
        beep4 = cube4.GetComponent<AudioSource>();
        beep5 = cube5.GetComponent<AudioSource>();

        seq = new int[100];
        kaka = Random.Range(1, numberOfCubes);
        seq[i] = kaka;
        current = seq[i];
        StartCoroutine(ExampleCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

        if (pushed == current)
        {

            if (seq[z + 1] == 0)
            {
                i++;
                addscore();
                //levelUp();
                kaka = Random.Range(1, numberOfCubes);
                seq[i] = kaka;
                a = 0;
                StartCoroutine(ExampleCoroutine());
                z = 0;
                current = seq[z];
            }

            else if (seq[z + 1] != 0)
            {
                z++;
                current = seq[z];
                a = 0;
            }
            pushed = 0;

        }

        else if (pushed > 0 && pushed != current)
        {
            gameover.text = "Game Over";
            Time.timeScale = 0f;
        }

    }

    IEnumerator ExampleCoroutine()
    {
        while (seq[a] != 0)
        {
            yield return new WaitForSeconds(timeBetweenBeep);
            if (seq[a] == 1)
            {
                cube1.GetComponent<Renderer>().material = cube1beep;
                beep1.Play();
                yield return new WaitForSeconds(timeCubeLit);
                cube1.GetComponent<Renderer>().material = cube1color;
                a++;
            }

            else if (seq[a] == 2)
            {
                cube2.GetComponent<Renderer>().material = cube2beep;
                beep2.Play();
                yield return new WaitForSeconds(timeCubeLit);
                cube2.GetComponent<Renderer>().material = cube2color;
                a++;
            }

            else if (seq[a] == 3)
            {
                cube3.GetComponent<Renderer>().material = cube3beep;
                beep3.Play();
                yield return new WaitForSeconds(timeCubeLit);
                cube3.GetComponent<Renderer>().material = cube3color;
                a++;
            }

            else if (seq[a] == 4)
            {
                cube4.GetComponent<Renderer>().material = cube4beep;
                beep4.Play();
                yield return new WaitForSeconds(timeCubeLit);
                cube4.GetComponent<Renderer>().material = cube4color;
                a++;
            }

            else if (seq[a] == 5)
            {
                cube5.GetComponent<Renderer>().material = cube5beep;
                beep5.Play();
                yield return new WaitForSeconds(timeCubeLit);
                cube5.GetComponent<Renderer>().material = cube5color;
                a++;
            }
        }
    }

    void addscore()
    {
        round++;
        number.text = round.ToString();
    }

    void levelUp()
    {
        if (round == 2)
        {
            cube1.transform.localPosition = new Vector3(0, 1.85f, 0);
            cube2.transform.localPosition = new Vector3(0.4f, 1.85f, 0);
            cube3.transform.localPosition = new Vector3(-0.4f, 1.85f, 0);
            cube3.SetActive(true);
            numberOfCubes = 4;
        }

        if (round == 3)
        {
            //cube4.SetActive(true);
            //numberOfCubes = 5;
        }
    }
}
