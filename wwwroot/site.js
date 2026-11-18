const API_URL = '/api/equipamentos';

// Alterna a visão ativa sem recarregar a página
function mostrarVisao(idVisao) {
    document.querySelectorAll('.view').forEach(sec => sec.classList.remove('active'));
    document.getElementById(idVisao).classList.add('active');

    if (idVisao === 'view-lista') {
        carregarEquipamentos();
    }
}

// 1. LISTAGEM (GET)
async function carregarEquipamentos() {
    try {
        const resposta = await fetch(API_URL);
        if (!resposta.ok) throw new Error('Falha ao carregar equipamentos.');

        const equipamentos = await resposta.json();
        const tbody = document.getElementById('tabela-equipamentos');
        tbody.innerHTML = '';

        if (equipamentos.length === 0) {
            tbody.innerHTML = '<tr><td colspan="5">Nenhum equipamento encontrado.</td></tr>';
            return;
        }

        equipamentos.forEach(eq => {
            const id = eq.id || eq._id;
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${id}</td>
                <td>${eq.nome || '-'}</td>
                <td>${eq.modelo || '-'}</td>
                <td>${eq.status || 'Ativo'}</td>
                <td><button onclick="verDetalhes('${id}')">Ver Detalhes</button></td>
            `;
            tbody.appendChild(tr);
        });
    } catch (erro) {
        console.error(erro);
        alert('Erro ao carregar a lista.');
    }
}

// 2. DETALHE DO ITEM (GET por ID)
async function verDetalhes(id) {
    try {
        const resposta = await fetch(`${API_URL}/${id}`);
        if (!resposta.ok) throw new Error('Equipamento não encontrado.');

        const eq = await resposta.json();
        const container = document.getElementById('conteudo-detalhes');
        container.innerHTML = `
            <p><strong>ID:</strong> ${eq.id || eq._id}</p>
            <p><strong>Nome:</strong> ${eq.nome || '-'}</p>
            <p><strong>Modelo:</strong> ${eq.modelo || '-'}</p>
            <p><strong>Status:</strong> ${eq.status || 'Ativo'}</p>
        `;
        mostrarVisao('view-detalhes');
    } catch (erro) {
        console.error(erro);
        alert('Não foi possível carregar os detalhes do item.');
    }
}

// 3. CADASTRO ASSÍNCRONO (POST)
async function salvarEquipamento(event) {
    event.preventDefault();

    const novoEquipamento = {
        nome: document.getElementById('nome').value,
        modelo: document.getElementById('modelo').value,
        status: document.getElementById('status').value
    };

    try {
        const resposta = await fetch(API_URL, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
                // Nota: Se o endpoint POST exigir JWT, adicione o header Authorization aqui
            },
            body: JSON.stringify(novoEquipamento)
        });

        if (!resposta.ok) {
            const erroTexto = await resposta.text();
            throw new Error(erroTexto || 'Erro ao cadastrar equipamento.');
        }

        document.getElementById('form-equipamento').reset();
        alert('Equipamento cadastrado com sucesso!');
        mostrarVisao('view-lista');
    } catch (erro) {
        console.error(erro);
        alert('Falha ao salvar: ' + erro.message);
    }
}

document.addEventListener('DOMContentLoaded', carregarEquipamentos);