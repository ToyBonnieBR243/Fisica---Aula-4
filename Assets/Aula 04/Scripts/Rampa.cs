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

    // Mathf.Sin/Cos trabalham em radianos; anguloGraus é o valor exposto no Inspector para o aluno
    // ajustar em graus. Esta é a conversão usada em toda fórmula de decomposição de forças.
    public float AnguloRad { get { return anguloGraus * Mathf.Deg2Rad; } }

    // Aplica o sinal de convenção da força externa F: positiva quando aponta rampa acima, negativa
    // quando aponta rampa abaixo. É esse valor com sinal que entra em F/m na equação de movimento.
    public float ForcaComSinal { get { return forcaRampaAcima ? forca : -forca; } }

    // Ângulo theta usado nas fórmulas de peso/normal/atrito em cada trecho do percurso: igual ao
    // ângulo da rampa (anguloGraus) quando s >= 0 (na rampa); zero no plano horizontal, já que ali
    // o peso não tem componente ao longo do movimento e a normal é igual ao próprio peso.
    public float AnguloEfetivo(float s) { return s >= 0f ? AnguloRad : 0f; }

    // Toda a física do exercício trabalha com um número só: s, a posição escalar ao longo do
    // percurso (não há vetores nas fórmulas de a, v, atrito etc). Este método é a ponte para o
    // mundo 2D/3D da Unity: devolve o versor tangente à pista em s, ou seja, para onde aponta o
    // sentido "positivo" (rampa acima) naquele trecho. Na rampa esse vetor sobe inclinado por
    // theta; no plano ele aponta para a esquerda, de volta em direção à base da rampa.
    public Vector3 SentidoPositivo(float s)
    {
        // Remover o default e implementar a função que retorna o versor tangente à pista em s, conforme descrito acima.
        return default(Vector3);
    }

    // Versor perpendicular à superfície de apoio, apontando para fora dela. É a direção da força
    // normal N usada em TetoDoAtritoEstatico/CalcularAceleracao (N = m.g.cos(theta)) e também serve
    // para levantar a caixa e deixá-la apoiada sobre a pista em vez de atravessá-la.
    public Vector3 Normal(float s)
    {
        // Remover o default e implementar a função que retorna o versor perpendicular à superfície de apoio em s, conforme descrito acima.
        return default(Vector3);
    }

    // Traduz a posição cinemática s (resultado da integração de velocidade/aceleração em
    // MovimentoCaixa) em coordenadas de mundo: anda "s" unidades a partir da base ao longo do
    // versor tangente da pista (rampa se s >= 0, plano se s < 0) e soma um deslocamento na direção
    // normal de meio lado da caixa, para ela ficar apoiada sobre a superfície e não com o centro
    // cravado nela.
    public Vector3 PosicaoDaCaixa(float s)
    {
        // Remover o default e implementar a função que retorna a posição da caixa em coordenadas de mundo, conforme descrito acima.
        return default(Vector3); 
    }

    // Alinha visualmente a caixa com a inclinação da superfície em que ela está apoiada: rotacionada
    // com base no ângulo theta na rampa (mesmo ângulo usado no cálculo de peso/normal/atrito), sem rotação no plano.
    public Quaternion RotacaoDaCaixa(float s)
    {
        // Remover o default e implementar a função que retorna a rotação da caixa em coordenadas de mundo, conforme descrito acima.
        return default(Quaternion); 
    }

    // Roda todo frame para atualizar a geometria da rampa, plano e caixa a partir dos parâmetros atuais 
    // (anguloGraus, comprimentoRampa etc.) editáveis em tempo real pelo Inspector durante o Play. 
    // Permite acompanhar o efeito de cada parâmetro nas fórmulas de atrito durante a execução.
    void Update()
    {
        
    }

    // "Redesenha" os objetos 3D (rampa, plano, caixa) a partir dos parâmetros atuais: a rampa é
    // orientada pelo mesmo AnguloRad/SentidoPositivo/Normal usados nas contas de força, então a
    // inclinação que o aluno vê sempre corresponde ao theta realmente usado na física.
    public void AtualizarGeometria()
    {
        
    }
}
