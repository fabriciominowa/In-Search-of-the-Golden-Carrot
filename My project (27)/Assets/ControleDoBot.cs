using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoBot : MonoBehaviour
{
    public float velocidade = 2.0f; // Velocidade de movimento do bot
    public float raioDeVisao = 3.0f; // Raio de visão do bot
    public float tempoParaAtacar = 1.0f; // Tempo mínimo para atacar após o jogador entrar no raio de visão
    public float tempoParaReativarAtaque = 20.0f; // Tempo para reativar o ataque após um ataque bem-sucedido

    private Transform jogador; // Referência ao objeto do jogador
    private bool podeCausarDano = true; // Para evitar múltiplos danos em rápida sucessão
    public int danoAoJogador = 1; // Quantidade de dano causada ao jogador

    private Vector3 posicaoOriginal; // Armazena a posição original do bot
    private bool esperandoParaAtacar = false; // Indica se o bot está esperando para atacar
    private float tempoNoRaioDeVisao = 0.0f; // Tempo que o jogador permaneceu no raio de visão

    private void Start()
    {
        jogador = GameObject.FindGameObjectWithTag("Player").transform; // Encontra o jogador na cena
        posicaoOriginal = transform.position; // Armazena a posição original do bot
    }

    private void Update()
    {
        if (esperandoParaAtacar)
        {
            return; // Não faz nada enquanto estiver esperando para atacar
        }

        // Verifica se o jogador está dentro do raio de visão
        float distanciaParaJogador = Vector2.Distance(transform.position, jogador.position);
        if (distanciaParaJogador <= raioDeVisao)
        {
            tempoNoRaioDeVisao += Time.deltaTime;

            // Se o jogador permanecer por tempoParaAtacar segundos no raio de visão, ataca
            if (tempoNoRaioDeVisao >= tempoParaAtacar && podeCausarDano)
            {
                CausarDanoAoJogador();
            }
        }
        else
        {
            tempoNoRaioDeVisao = 0.0f; // Reinicia o contador quando o jogador sai do raio de visão
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
                tempoNoRaioDeVisao = 0.0f; // Reinicia o contador quando o ataque é realizado
                Invoke("ReativarDano", tempoParaReativarAtaque); // Reativa a capacidade de causar dano após tempoParaReativarAtaque segundos
            }
        }

        // Retorna o bot para a posição original
        transform.position = posicaoOriginal;
    }

    private void ReativarDano()
    {
        // Reativa a capacidade de causar dano e permite que o bot continue perseguindo o jogador
        podeCausarDano = true;
        esperandoParaAtacar = false;
    }
}