const API_URL = '/api/equipamentos'; // Ajuste conforme a rota da sua API

async function carregarEquipamentos() {
    try {
        const resposta = await fetch(API_URL);
        if (!resposta.ok) {
            throw new Error('Erro ao buscar dados da API.');
        }

        const equipamentos = await resposta.json();
        const tbody = document.getElementById('tabela-equipamentos');
        tbody.innerHTML = ''; // Limpa a tabela antes de renderizar

        if (equipamentos.length === 0) {
            tbody.innerHTML = '<tr><td colspan="4">Nenhum equipamento cadastrado.</td></tr>';
            return;
        }

        equipamentos.forEach(eq => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${eq.id || eq._id}</td>
                <td>${eq.nome || 'Sem nome'}</td>
                <td>${eq.status || 'Ativo'}</td>
                <td><button onclick="verDetalhes('${eq.id || eq._id}')">Detalhes</button></td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Erro:', error);
        alert('Não foi possível carregar a lista de equipamentos.');
    }
}

function verDetalhes(id) {
    alert(`Navegando para os detalhes do item ID: ${id} (Navegação assíncrona)`);
    // Aqui você implementará a transição de visão (Lista -> Detalhe) no Commit 16
}

// Carregar automaticamente ao abrir a página
document.addEventListener('DOMContentLoaded', carregarEquipamentos);