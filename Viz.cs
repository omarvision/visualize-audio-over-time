using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Viz : MonoBehaviour
{
    
    public int numSamples = 128;
    public float meterScale = 100f;
    public FFTWindow fft = FFTWindow.Rectangular;
    public float[] samples = null;
    private GameObject[] quads = null;
    private AudioSource aud = null;
        
    private void Start()
    {
        //reference to audio
        aud = this.GetComponent<AudioSource>();

        //allocate
        samples = new float[numSamples];
        quads = new GameObject[numSamples];

        CreateQuads();
    }
    private void Update()
    {
        //get samples
        aud.GetSpectrumData(samples, 0, fft);

        //show the samples
        for (int i = 0; i < numSamples; i++)
        {
            float val = meterScale * samples[i];
            quads[i].transform.localScale = new Vector3(1, val, 1);
        }
    }    
    private void CreateQuads()
    {
        //random color material
        Material mat = new Material(Shader.Find("Specular"));
        mat.color = Random.ColorHSV();

        //make the gameobjects to show samples
        for (int i = 0; i < numSamples; i++)
        {
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "quad" + i.ToString().PadLeft(3, '0');            
            quad.transform.position = new Vector3(i, this.transform.position.y, 0);
            quad.transform.parent = this.transform;     
            
            quad.GetComponent<Renderer>().material = mat;

            quads[i] = quad;
        }
    }
}
