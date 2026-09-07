using UnityEngine;

// Toda a física da caixa. Nenhum Rigidbody, nenhum Collider[cite: 8].
[RequireComponent(typeof(Rampa))]
public class MovimentoCaixa : MonoBehaviour
{
    Rampa b;

    [Header("Estado (somente leitura)")]
    public float s;
    public float velocidade;
    public float aceleracao;
    public bool parada = true;
    public float distanciaPercorrida;
    public float velocidadeNaBaseDaRampa;

    const float EPS = 1e-4f;

    void Awake()
    {
        b = GetComponent<Rampa>();
    }

    void Start()
    {
        Soltar();
    }

    public void Soltar()
    {
        s = b.comprimentoRampa; // Topo da rampa[cite: 8]
        velocidade = 0f;
        aceleracao = 0f;
        parada = true;
        distanciaPercorrida = 0f;
        velocidadeNaBaseDaRampa = 0f;
        AtualizarPosicao();
    }

    public void Lancar(float v0)
    {
        s = 0f; // Base da rampa[cite: 8]
        velocidade = Mathf.Abs(v0); // Sempre tratada como rampa acima[cite: 8]
        aceleracao = 0f;
        parada = false;
        distanciaPercorrida = 0f;
        velocidadeNaBaseDaRampa = velocidade;
        AtualizarPosicao();
    }

    public float ResultanteParalelaSemAtrito()
    {
        // Força F menos a componente do peso ao longo da rampa (m*g*sen(theta))[cite: 8].
        return b.ForcaComSinal - (b.massa * Rampa.G * Mathf.Sin(b.AnguloEfetivo(s)));
    }

    public float TetoDoAtritoEstatico()
    {
        // fat_max = mu_e * N, onde N = m*g*cos(theta)[cite: 5, 8].
        return b.muE * b.massa * Rampa.G * Mathf.Cos(b.AnguloEfetivo(s));
    }

    public bool DeveIniciarMovimento()
    {
        // A caixa só sai do repouso quando a força supera o atrito estático máximo[cite: 5, 8].
        return Mathf.Abs(ResultanteParalelaSemAtrito()) > TetoDoAtritoEstatico();
    }

    public float SentidoDoDeslizamento()
    {
        // Com a caixa parada, usa o sinal da resultante sem atrito para prever o sentido[cite: 8].
        if (parada) return Mathf.Sign(ResultanteParalelaSemAtrito());
        if (Mathf.Abs(velocidade) < EPS) return 0f;
        return Mathf.Sign(velocidade);
    }

    public float CalcularAceleracao()
    {
        float theta = b.AnguloEfetivo(s);
        // a = F/m - g.sen(theta) - sinal(v).mu_c.g.cos(theta)[cite: 5, 8]
        float termoForca = b.ForcaComSinal / b.massa;
        float termoGravidade = Rampa.G * Mathf.Sin(theta);
        float termoAtrito = SentidoDoDeslizamento() * b.muC * Rampa.G * Mathf.Cos(theta);

        return termoForca - termoGravidade - termoAtrito;
    }

    void FixedUpdate()
    {
        if (parada)
        {
            if (DeveIniciarMovimento())
            {
                parada = false;
            }
            else
            {
                aceleracao = 0f;
                velocidade = 0f;
                return;
            }
        }

        aceleracao = CalcularAceleracao();

        // Método de Euler semi-implícito[cite: 8]
        float novaVelocidade = velocidade + aceleracao * Time.fixedDeltaTime;

        // Se o sinal da velocidade se inverteu, a caixa zerou a velocidade neste frame.
        // Travamos em zero para que o atrito estático seja reavaliado no próximo frame[cite: 4, 8].
        if ((velocidade > 0f && novaVelocidade < 0f) || (velocidade < 0f && novaVelocidade > 0f))
        {
            velocidade = 0f;
            parada = true;
        }
        else
        {
            velocidade = novaVelocidade;
        }

        float ds = velocidade * Time.fixedDeltaTime;

        // Verifica se cruzou a base (s = 0) neste frame para registrar a velocidade exata[cite: 4, 8].
        if ((s > 0f && s + ds <= 0f) || (s < 0f && s + ds >= 0f))
        {
            velocidadeNaBaseDaRampa = Mathf.Abs(velocidade);
        }

        s += ds; // Atualiza a posição (s += v.dt)[cite: 8]
        distanciaPercorrida += Mathf.Abs(ds);

        AtualizarPosicao();
    }

    // Aplica o "s" atual na posição e rotação do Transform da caixa na cena[cite: 8].
    void AtualizarPosicao()
    {
        b.caixa.position = b.PosicaoDaCaixa(s);
        b.caixa.rotation = b.RotacaoDaCaixa(s);
    }
}