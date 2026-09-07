using UnityEngine;

// Guarda os parâmetros da rampa e converte a posição escalar "s"
// em posição no mundo. Não faz física: quem faz é MovimentoCaixa.
public class Rampa : MonoBehaviour
{
    public const float G = 9.8f;

    [Header("Parametros ajustaveis em tempo de execucao")]
    [Range(0f, 45f)] public float anguloGraus = 30f;
    [Range(0f, 1.2f)] public float muE = 0.35f;
    [Range(0f, 1.2f)] public float muC = 0.20f;
    [Range(0.5f, 50f)] public float massa = 2f;
    [Range(0f, 200f)] public float forca = 0f;
    public bool forcaRampaAcima = true;

    [Header("Geometria da rampa")]
    public float comprimentoRampa = 4f;
    public float comprimentoPlano = 10f;
    public float larguraPista = 2f;
    public float espessuraPista = 0.2f;
    public float ladoCaixa = 0.6f;

    [Header("Objetos da cena")]
    public Transform rampa;
    public Transform plano;
    public Transform caixa;

    public float AnguloRad { get { return anguloGraus * Mathf.Deg2Rad; } }

    public float ForcaComSinal { get { return forcaRampaAcima ? forca : -forca; } }

    public float AnguloEfetivo(float s) { return s >= 0f ? AnguloRad : 0f; }

    public Vector3 SentidoPositivo(float s)
    {
        // Na rampa (s >= 0), o versor sobe inclinado para a esquerda. 
        // No plano (s < 0), ele aponta para a esquerda, em direção à base[cite: 8].
        if (s >= 0f)
            return new Vector3(-Mathf.Cos(AnguloRad), Mathf.Sin(AnguloRad), 0f);
        else
            return Vector3.left;
    }

    public Vector3 Normal(float s)
    {
        // Versor perpendicular à superfície de apoio[cite: 8].
        if (s >= 0f)
            return new Vector3(Mathf.Sin(AnguloRad), Mathf.Cos(AnguloRad), 0f);
        else
            return Vector3.up;
    }

    public Vector3 PosicaoDaCaixa(float s)
    {
        // Anda "s" unidades a partir da base ao longo do versor tangente e soma um 
        // deslocamento na direção normal de meio lado da caixa[cite: 8].
        Vector3 basePos = transform.position; // Assume que a origem deste script é a base
        return basePos + (s * SentidoPositivo(s)) + (Normal(s) * (ladoCaixa / 2f));
    }

    public Quaternion RotacaoDaCaixa(float s)
    {
        // A inclinação caindo para a direita em 2D representa um ângulo negativo no eixo Z[cite: 8].
        return s >= 0f ? Quaternion.Euler(0f, 0f, -anguloGraus) : Quaternion.identity;
    }

    void Update()
    {
        AtualizarGeometria();
    }

    public void AtualizarGeometria()
    {
        if (rampa != null)
        {
            rampa.position = transform.position + SentidoPositivo(1f) * (comprimentoRampa / 2f) - Normal(1f) * (espessuraPista / 2f);
            rampa.rotation = Quaternion.Euler(0, 0, -anguloGraus);
            rampa.localScale = new Vector3(comprimentoRampa, espessuraPista, 1f);
        }
        if (plano != null)
        {
            plano.position = transform.position + Vector3.right * (comprimentoPlano / 2f) - Vector3.up * (espessuraPista / 2f);
            plano.rotation = Quaternion.identity;
            plano.localScale = new Vector3(comprimentoPlano, espessuraPista, 1f);
        }
        if (caixa != null)
        {
            caixa.localScale = new Vector3(ladoCaixa, ladoCaixa, 1f);
        }
    }
}