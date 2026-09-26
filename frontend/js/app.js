import { taskApi } from './apiClient.js';


const taskForm = document.getElementById('taskForm');
const taskTitle = document.getElementById('taskTitle');
const taskPriority = document.getElementById('taskPriority');
const submitBtn = document.getElementById('submitBtn');
const formError = document.getElementById('formError');
const taskList = document.getElementById('taskList');
const emptyState = document.getElementById('emptyState');


async function loadTasks() {
    try {
        formError.textContent = '';
        const tasks = await taskApi.getAll();
        renderTasks(tasks);
    } catch (err) {
        formError.textContent = 'Görevler yüklenirken bir hata oluştu: ' + err.message;
    }
}


function renderTasks(tasks) {
    taskList.innerHTML = '';

    
    if (!tasks || tasks.length === 0) {
        emptyState.style.display = 'block';
        return;
    }
    emptyState.style.display = 'none';

    tasks.forEach(task => {
        const item = document.createElement('div');
        item.className = 'task-card';

        
        const badgeClass = task.priority ? `badge-${task.priority.toLowerCase()}` : 'badge-normal';

        item.innerHTML = `
            <div class="task-info">
                <strong>${escapeHtml(task.title)}</strong>
                <span class="badge ${badgeClass}">${task.priority || 'normal'}</span>
            </div>
            <div>
                <button class="delete-btn" data-id="${task.id}">Sil</button>
            </div>
        `;

        taskList.appendChild(item);
    });

    
    bindDeleteButtons();
}


function bindDeleteButtons() {
    const deleteButtons = document.querySelectorAll('.delete-btn');
    deleteButtons.forEach(btn => {
        btn.addEventListener('click', async () => {
            const taskId = btn.dataset.id;
            await deleteTask(taskId, btn);
        });
    });
}


taskForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    formError.textContent = '';

    const titleValue = taskTitle.value.trim();
    if (!titleValue) {
        formError.textContent = 'Lütfen geçerli bir görev başlığı girin.';
        return;
    }

    
    submitBtn.disabled = true;
    submitBtn.textContent = 'Ekleniyor...';

    const payload = {
        title: titleValue,
        priority: taskPriority.value
    };

    try {
        await taskApi.create(payload);
        taskTitle.value = '';
        taskPriority.value = 'normal';
        await loadTasks(); 
    } catch (err) {
        
        formError.textContent = 'Görev eklenemedi: ' + err.message;
    } finally {
        
        submitBtn.disabled = false;
        submitBtn.textContent = 'Görev Ekle';
    }
});


async function deleteTask(id, btnElement) {
    
    const isConfirmed = confirm('Bu görevi silmek istediğinize emin misiniz?');
    if (!isConfirmed) return;

    
    if (btnElement) {
        btnElement.disabled = true;
        btnElement.textContent = 'Siliniyor...';
    }

    try {
        await taskApi.delete(id);
        await loadTasks(); 
    } catch (err) {
        alert('Silme işlemi başarısız: ' + err.message);
        if (btnElement) {
            btnElement.disabled = false;
            btnElement.textContent = 'Sil';
        }
    }
}


function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}


document.addEventListener('DOMContentLoaded', loadTasks);