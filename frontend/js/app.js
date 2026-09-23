

let tasks = [];


tasks = [
    {
        id: 1727000000001,
        title: "Veritabanı şema tasarımını tamamla",
        priority: "high",
        status: "open",
        createdAt: "2026-09-20"
    },
    {
        id: 1727000000002,
        title: "Kritik güvenlik açıklarını test et",
        priority: "high",
        status: "open",
        createdAt: "2026-09-21"
    },
    {
        id: 1727000000003,
        title: "CSS Grid ve Flexbox düzenini oluştur",
        priority: "normal",
        status: "completed",
        createdAt: "2026-09-19"
    }
];


const form = document.querySelector('#task-form');
const titleInput = document.querySelector('#title');
const prioritySelect = document.querySelector('#priority');
const tableBody = document.querySelector('#task-table-body');
const tableWrapper = document.querySelector('#table-wrapper');
const emptyState = document.querySelector('#empty-state');

const statusFilter = document.querySelector('#status-filter');
const priorityFilter = document.querySelector('#priority-filter');

const totalTasksEl = document.querySelector('#total-tasks');
const openTasksEl = document.querySelector('#open-tasks');
const completedTasksEl = document.querySelector('#completed-tasks');


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
        let isHighPriority = task.priority === 'high';
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
            applyFilterAndRender();
        }
    }
});


statusFilter.addEventListener('change', applyFilterAndRender);
priorityFilter.addEventListener('change', applyFilterAndRender);


applyFilterAndRender();