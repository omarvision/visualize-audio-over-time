using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VizOverTime : MonoBehaviour
{
    public int numSamples = 64;
    public int numHistory = 32;
    public float meterScale = 100f;
    public float[] samples = null;
    private GameObject[] lines = null;
    private AudioSource aud = null;
    private int cntSamplesShow = 0;

    private void Start()
    {
        aud = this.GetComponent<AudioSource>();
        samples = new float[numSamples];
        cntSamplesShow = Mathf.RoundToInt(numSamples * 0.5f);
        lines = new GameObject[numHistory]; 
        CreateObjects();
    }
    private void Update()
    {
        aud.GetSpectrumData(samples, 0, FFTWindow.Rectangular);
        Visualize();
    }
    private void CreateObjects()
    {
        Material mat = new Material(Shader.Find("Specular"));
        mat.color = Random.ColorHSV();

        for (int h = 0; h < numHistory; h++)
        {
            // 1 - new line
            GameObject line = new GameObject();
            line.transform.position = new Vector3(0, 0, h);
            line.transform.parent = this.transform;
            line.name = "line_" + h.ToString().PadLeft(3, '0');

            for (int s = 0; s < cntSamplesShow; s++)
            {
                // 2 - new quads on line (one quad per sample)
                GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                quad.transform.parent = line.transform;
                quad.transform.position = new Vector3(s, 0, h);
                quad.GetComponent<Renderer>().material = mat;
                quad.name = "quad_" + s.ToString().PadLeft(3, '0');
            }

            lines[h] = line;
        }
    }
    private void Visualize()
    {
        GameObject closestline = null;

        // shift lines
        for (int h = 0; h < numHistory; h++)
        {
            lines[h].transform.Translate(Vector3.forward);
            if (lines[h].transform.position.z > numHistory)
            {
                lines[h].transform.position = new Vector3(0, 0, 0);
                closestline = lines[h];
            }
        }

        // visualize samples on the current closest line
        if (closestline != null)
        {
            for (int s = 0; s < cntSamplesShow; s++)
            {
                float value = samples[s] * meterScale;
                closestline.transform.GetChild(s).localScale = new Vector3(1, value, 1);
            }
        }
    }

}
