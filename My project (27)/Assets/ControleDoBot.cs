using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoBot : MonoBehaviour
{
    public float velocidade = 2.0f; // Velocidade de movimento do bot
    public float raioDeVisao = 3.0f; // Raio de vis�o do bot
    public float tempoParaAtacar = 1.0f; // Tempo m�nimo para atacar ap�s o jogador entrar no raio de vis�o
    public float tempoParaReativarAtaque = 20.0f; // Tempo para reativar o ataque ap�s um ataque bem-sucedido

    private Transform jogador; // Refer�ncia ao objeto do jogador
    private bool podeCausarDano = true; // Para evitar m�ltiplos danos em r�pida sucess�o
    public int danoAoJogador = 1; // Quantidade de dano causada ao jogador

    private Vector3 posicaoOriginal; // Armazena a posi��o original do bot
    private bool esperandoParaAtacar = false; // Indica se o bot est� esperando para atacar
    private float tempoNoRaioDeVisao = 0.0f; // Tempo que o jogador permaneceu no raio de vis�o

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform; // Encontra o jogador na cena
        posicaoOriginal = transform.position; // Armazena a posi��o original do bot
    }

    private void Update()
    {
        if (esperandoParaAtacar)
        {
            return; // N�o faz nada enquanto estiver esperando para atacar
        }

        // Verifica se o jogador est� dentro do raio de vis�o
        float distanciaParaJogador = Vector2.Distance(transform.position, jogador.position);
        if (distanciaParaJogador <= raioDeVisao)
        {
            tempoNoRaioDeVisao += Time.deltaTime;

            // Se o jogador permanecer por tempoParaAtacar segundos no raio de vis�o, ataca
            if (tempoNoRaioDeVisao >= tempoParaAtacar && podeCausarDano)
            {
                CausarDanoAoJogador();
            }
        }
        else
        {
            tempoNoRaioDeVisao = 0.0f; // Reinicia o contador quando o jogador sai do raio de vis�o
        }
    }

    private void CausarDanoAoJogador()
    {
        if (jogador != null)
        {
            ControleDoJogador controleJogador = jogador.GetComponent<ControleDoJogador>();
            if (controleJogador != null)
            {
                controleJogador.TomarDano(danoAoJogador); // Causa dano ao jogador
                podeCausarDano = false;
                esperandoParaAtacar = true;
                tempoNoRaioDeVisao = 0.0f; // Reinicia o contador quando o ataque � realizado
                Invoke("ReativarDano", tempoParaReativarAtaque); // Reativa a capacidade de causar dano ap�s tempoParaReativarAtaque segundos
            }
        }

        // Retorna o bot para a posi��o original
        transform.position = posicaoOriginal;
    }

    private void ReativarDano()
    {
        // Reativa a capacidade de causar dano e permite que o bot continue perseguindo o jogador
        podeCausarDano = true;
        esperandoParaAtacar = false;
    }
}