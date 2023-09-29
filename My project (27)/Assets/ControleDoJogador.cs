using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControleDoJogador : MonoBehaviour
{
    public float velocidade = 5.0f;
    public float forcaPulo = 8.0f;
    public int vidaJogador = 3;

    private Rigidbody2D rb2D;
    private Animator animator;

    private bool estaAndando = false;
    private bool estaPulando = false;
    private bool estaMorto = false; // Adicione uma vari�vel para rastrear se o jogador est� morto.

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Verifica se o jogador est� no ch�o
        bool estaNoChao = Physics2D.Raycast(transform.position, Vector2.down, 0.1f);

        if (!estaMorto) // Verifique se o jogador n�o est� morto antes de permitir movimento.
        {
            float movimentoHorizontal = Input.GetAxis("Horizontal");
            Vector3 direcaoJogador = new Vector3(movimentoHorizontal, 0.0f, 0.0f);
            transform.Translate(direcaoJogador * velocidade * Time.deltaTime);

            estaAndando = Mathf.Abs(movimentoHorizontal) > 0.1f;

            if (estaNoChao && Input.GetButtonDown("Jump"))
            {
                rb2D.AddForce(Vector2.up * forcaPulo, ForceMode2D.Impulse);
                estaPulando = true;
            }
            else
            {
                estaPulando = false;
            }
        }

        AtualizarAnimacoes();
    }

    private void AtualizarAnimacoes()
    {
        animator.SetBool("andando", estaAndando);
        animator.SetBool("Parado", !estaAndando); // "Parado" � o oposto de "Andando".
        animator.SetBool("Pulando", estaPulando);
        animator.SetBool("Morto", estaMorto); // Ative a anima��o de "morto" quando o jogador est� morto.
    }

    // Detec��o de dano
    public void TomarDano(int quantidade)
    {
        vidaJogador -= quantidade;
        if (vidaJogador <= 0)
        {
            // O jogador morreu.
            estaMorto = true; // Ative a anima��o de "morto".
            // Voc� pode adicionar mais l�gica aqui, como encerrar o jogo ou reiniciar.
        }
    }
}