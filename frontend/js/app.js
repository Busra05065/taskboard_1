


let tasks = [];


const form = document.querySelector('#task-form');
const titleInput = document.querySelector('#title');
const prioritySelect = document.querySelector('#priority');
const tableBody = document.querySelector('#task-table-body');
const tableWrapper = document.querySelector('#table-wrapper');
const emptyState = document.querySelector('#empty-state');
const statusAlert = document.querySelector('#status-alert');

const statusFilter = document.querySelector('#status-filter');
const priorityFilter = document.querySelector('#priority-filter');

const btnFetchSample = document.querySelector('#btn-fetch-sample');
const btnClearStorage = document.querySelector('#btn-clear-storage');

const totalTasksEl = document.querySelector('#total-tasks');
const openTasksEl = document.querySelector('#open-tasks');
const completedTasksEl = document.querySelector('#completed-tasks');


function showAlert(message, type = 'info', timeout = 4000) {
    statusAlert.textContent = message;
    statusAlert.className = `alert-box ${type}`;
    statusAlert.hidden = false;

    if (timeout > 0) {
        setTimeout(() => {
            statusAlert.hidden = true;
        }, timeout);
    }
}


function saveTasks() {
    localStorage.setItem('tasks', JSON.stringify(tasks));
}

function loadTasks() {
    const raw = localStorage.getItem('tasks');
    if (!raw) {
        tasks = [];
        return;
    }

    try {
        tasks = JSON.parse(raw);
        if (!Array.isArray(tasks)) {
            tasks = [];
        }
    } catch (error) {
        console.error("LocalStorage JSON parse hatası:", error);
        showAlert("Kayıtlı görev verisi bozuk olduğu için sıfırlandı.", "error");
        tasks = [];
        saveTasks();
    }
}


async function loadSampleTasks() {
    showAlert("Örnek görevler yükleniyor...", "info", 0);
    try {
        const response = await fetch('./data/tasks.json');
        if (!response.ok) {
            throw new Error(`Dosya okunamadı: HTTP ${response.status}`);
        }
        const sampleData = await response.json();

        
        tasks = [...sampleData, ...tasks];
        saveTasks();
        applyFilterAndRender();
        showAlert("Örnek görevler başarıyla aktarıldı!", "success");
    } catch (error) {
        console.error("Fetch Hatası:", error);
        showAlert(`Örnek görevler yüklenemedi: ${error.message}`, "error");
    }
}


function createTask(title, priority) {
    return {
        id: Date.now(),
        title: title,
        priority: priority,
        status: 'open',
        createdAt: new Date().toLocaleDateString('tr-TR')
    };
}


function updateKPIs() {
    const totalCount = tasks.length;
    const openCount = tasks.filter(t => t.status === 'open').length;
    const completedCount = tasks.filter(t => t.status === 'completed').length;

    totalTasksEl.textContent = totalCount;
    openTasksEl.textContent = openCount;
    completedTasksEl.textContent = completedCount;
}


function renderTasks(items) {
    if (items.length === 0) {
        tableWrapper.hidden = true;
        emptyState.hidden = false;
        tableBody.innerHTML = '';
        return;
    }

    tableWrapper.hidden = false;
    emptyState.hidden = true;

    tableBody.innerHTML = items.map(task => {
        let priorityBadge = '';
        const isHighPriority = task.priority === 'high';

        if (task.priority === 'high') {
            priorityBadge = '<span class="badge badge-high">Yüksek</span>';
        } else if (task.priority === 'normal') {
            priorityBadge = '<span class="badge badge-normal">Normal</span>';
        } else {
            priorityBadge = '<span class="badge badge-low">Düşük</span>';
        }

        const statusBadge = task.status === 'completed'
            ? '<span class="status status-completed">Tamamlandı</span>'
            : '<span class="status status-open">Açık</span>';

        const actionButton = task.status === 'completed'
            ? '<button class="btn-action" disabled>Tamamlandı</button>'
            : `<button class="btn-action" data-id="${task.id}">Tamamla</button>`;

        return `
            <tr class="${isHighPriority ? 'priority-high-row' : ''}">
                <td class="${task.status === 'completed' ? 'completed-text' : ''}">
                    ${isHighPriority ? `<strong>${task.title}</strong>` : task.title}
                </td>
                <td>${priorityBadge}</td>
                <td>${statusBadge}</td>
                <td>${actionButton}</td>
            </tr>
        `;
    }).join('');
}


function getFilteredTasks() {
    const selectedStatus = statusFilter.value;
    const selectedPriority = priorityFilter.value;

    return tasks.filter(task => {
        const matchesStatus = (selectedStatus === 'all') || (task.status === selectedStatus);
        const matchesPriority = (selectedPriority === 'all') || (task.priority === selectedPriority);
        return matchesStatus && matchesPriority;
    });
}

function applyFilterAndRender() {
    const filtered = getFilteredTasks();
    renderTasks(filtered);
    updateKPIs();
}




form.addEventListener('submit', function (event) {
    event.preventDefault();

    const title = titleInput.value.trim();
    const priority = prioritySelect.value;

    if (!title) return;

    const newTask = createTask(title, priority);
    tasks.unshift(newTask);
    saveTasks(); 

    form.reset();
    titleInput.focus();

    applyFilterAndRender();
});


tableBody.addEventListener('click', function (event) {
    if (event.target.classList.contains('btn-action') && !event.target.disabled) {
        const taskId = Number(event.target.getAttribute('data-id'));
        const task = tasks.find(t => t.id === taskId);
        if (task) {
            task.status = 'completed';
            saveTasks(); 
            applyFilterAndRender();
        }
    }
});


statusFilter.addEventListener('change', applyFilterAndRender);
priorityFilter.addEventListener('change', applyFilterAndRender);

btnFetchSample.addEventListener('click', loadSampleTasks);


btnClearStorage.addEventListener('click', function () {
    if (confirm("Tüm görevleri silmek istediğinizden emin misiniz?")) {
        tasks = [];
        localStorage.removeItem('tasks');
        applyFilterAndRender();
        showAlert("Tüm görevler ve yerel hafıza temizlendi.", "info");
    }
});


const API_URL = "http://localhost:5250/api/tasks";


async function fetchTasksFromAPI() {
    try {
        const response = await fetch(API_URL);
        if (!response.ok) {
            throw new Error(`Sunucu hatası: ${response.status}`);
        }
        const data = await response.json();
        tasks = data; 
        applyFilterAndRender();
    } catch (error) {
        
        console.error("API Hatası:", error);
        showAlert("Görevler sunucudan alınamadı. API servisinin açık olduğundan emin olun.", "danger");
    }
}


fetchTasksFromAPI();