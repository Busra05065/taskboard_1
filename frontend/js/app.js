import { taskApi, authApi } from './apiClient.js';

let currentUser = { isAuthenticated: false, username: null, role: null };


let queryState = {
    search: '',
    priority: '',
    sortBy: 'desc',
    page: 1,
    pageSize: 10
};

let totalPages = 1;


const loginForm = document.getElementById('loginForm');
const userProfile = document.getElementById('userProfile');
const loginUsername = document.getElementById('loginUsername');
const loginPassword = document.getElementById('loginPassword');
const displayUsername = document.getElementById('displayUsername');
const displayRole = document.getElementById('displayRole');
const logoutBtn = document.getElementById('logoutBtn');
const authError = document.getElementById('authError');

const taskForm = document.getElementById('taskForm');
const taskTitle = document.getElementById('taskTitle');
const taskPriority = document.getElementById('taskPriority');
const submitBtn = document.getElementById('submitBtn');
const formError = document.getElementById('formError');

const searchInput = document.getElementById('searchInput');
const priorityFilter = document.getElementById('priorityFilter');
const sortFilter = document.getElementById('sortFilter');
const pageSizeFilter = document.getElementById('pageSizeFilter');

const taskList = document.getElementById('taskList');
const emptyState = document.getElementById('emptyState');
const totalInfo = document.getElementById('totalInfo');
const prevPageBtn = document.getElementById('prevPageBtn');
const nextPageBtn = document.getElementById('nextPageBtn');


async function checkAuth() {
    try {
        const user = await authApi.getCurrentUser();
        currentUser = user;
        updateAuthUI();
    } catch (_) {
        currentUser = { isAuthenticated: false };
        updateAuthUI();
    }
}

function updateAuthUI() {
    authError.textContent = '';
    if (currentUser.isAuthenticated) {
        loginForm.style.display = 'none';
        userProfile.style.display = 'flex';
        displayUsername.textContent = currentUser.username;
        displayRole.textContent = currentUser.role;
    } else {
        loginForm.style.display = 'flex';
        userProfile.style.display = 'none';
    }
    loadTasks();
}

loginForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    authError.textContent = '';
    try {
        await authApi.login(loginUsername.value, loginPassword.value);
        loginUsername.value = '';
        loginPassword.value = '';
        await checkAuth();
    } catch (err) {
        authError.textContent = err.message;
    }
});

logoutBtn.addEventListener('click', async () => {
    await authApi.logout();
    await checkAuth();
});


async function loadTasks() {
    try {
        formError.textContent = '';
        const data = await taskApi.getAll(queryState);
        renderTasks(data);
    } catch (err) {
        formError.textContent = 'Görevler yüklenemedi: ' + err.message;
    }
}

function renderTasks(data) {
    taskList.innerHTML = '';
    const { items, totalCount, page, pageSize, totalPages: pages } = data;
    totalPages = pages || 1;

    
    totalInfo.textContent = `Toplam ${totalCount} kayıt | Sayfa ${page}/${totalPages}`;
    prevPageBtn.disabled = page <= 1;
    nextPageBtn.disabled = page >= totalPages;

    
    if (!items || items.length === 0) {
        emptyState.style.display = 'block';
        if (queryState.search) {
            emptyState.textContent = `"${queryState.search}" aramasına uygun görev bulunamadı.`;
        } else {
            emptyState.textContent = 'Henüz eklenmiş bir görev bulunmuyor.';
        }
        return;
    }

    emptyState.style.display = 'none';

    items.forEach(task => {
        const item = document.createElement('div');
        item.className = 'task-card';

        const deleteButtonHtml = currentUser.role === 'Admin'
            ? `<button class="btn btn-danger delete-btn" data-id="${task.id}">Sil</button>`
            : '';

        item.innerHTML = `
            <div>
                <strong>${escapeHtml(task.title)}</strong>
                <span style="font-size: 12px; color: gray; margin-left: 6px;">[${task.priority || 'normal'}]</span>
            </div>
            <div>${deleteButtonHtml}</div>
        `;

        taskList.appendChild(item);
    });

    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', () => deleteTask(btn.dataset.id, btn));
    });
}


let debounceTimer;
searchInput.addEventListener('input', () => {
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(() => {
        queryState.search = searchInput.value;
        queryState.page = 1;
        loadTasks();
    }, 300);
});


priorityFilter.addEventListener('change', () => {
    queryState.priority = priorityFilter.value;
    queryState.page = 1;
    loadTasks();
});

sortFilter.addEventListener('change', () => {
    queryState.sortBy = sortFilter.value;
    queryState.page = 1;
    loadTasks();
});

pageSizeFilter.addEventListener('change', () => {
    queryState.pageSize = parseInt(pageSizeFilter.value);
    queryState.page = 1;
    loadTasks();
});


prevPageBtn.addEventListener('click', () => {
    if (queryState.page > 1) {
        queryState.page--;
        loadTasks();
    }
});

nextPageBtn.addEventListener('click', () => {
    if (queryState.page < totalPages) {
        queryState.page++;
        loadTasks();
    }
});


taskForm.addEventListener('submit', async (e) => {
    e.preventDefault();
    formError.textContent = '';

    const titleValue = taskTitle.value.trim();
    if (!titleValue) {
        formError.textContent = 'Görev başlığı boş bırakılamaz.';
        return;
    }

    submitBtn.disabled = true;
    try {
        await taskApi.create({
            title: titleValue,
            priority: taskPriority.value
        });
        taskTitle.value = '';
        taskPriority.value = 'normal';
        queryState.page = 1;
        await loadTasks();
    } catch (err) {
        formError.textContent = err.message;
    } finally {
        submitBtn.disabled = false;
    }
});


async function deleteTask(id, btnElement) {
    if (!confirm('Bu görevi silmek istediğinize emin misiniz?')) return;
    if (btnElement) btnElement.disabled = true;

    try {
        await taskApi.delete(id);
        await loadTasks();
    } catch (err) {
        alert(err.message);
        if (btnElement) btnElement.disabled = false;
    }
}

function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

document.addEventListener('DOMContentLoaded', () => {
    checkAuth();
});