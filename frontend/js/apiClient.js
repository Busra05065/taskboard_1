const API_BASE_URL = 'http://localhost:5250';

async function request(endpoint, options = {}) {
    const url = `${API_BASE_URL}${endpoint}`;
    const headers = {
        'Content-Type': 'application/json',
        ...options.headers
    };

    const response = await fetch(url, { 
        ...options, 
        headers,
        credentials: 'include' 
    });

    if (response.status === 401) {
        throw new Error('Lütfen önce giriş yapın.');
    }
    if (response.status === 403) {
        throw new Error('Bu işlemi yapmaya yetkiniz yok! (Yalnızca Admin silebilir)');
    }

    if (!response.ok) {
        let errorMsg = 'İşlem başarısız oldu';
        try {
            const errData = await response.json();
            if (errData && errData.message) errorMsg = errData.message;
        } catch (_) {}
        throw new Error(errorMsg);
    }

    if (response.status === 204) {
        return null;
    }

    return await response.json();
}

export const taskApi = {
    getAll: () => request('/api/tasks'),
    create: (data) => request('/api/tasks', { method: 'POST', body: JSON.stringify(data) }),
    update: (id, data) => request(`/api/tasks/${id}`, { method: 'PUT', body: JSON.stringify(data) }),
    delete: (id) => request(`/api/tasks/${id}`, { method: 'DELETE' })
};

export const authApi = {
    login: (username, password) => request('/api/auth/login', { method: 'POST', body: JSON.stringify({ username, password }) }),
    logout: () => request('/api/auth/logout', { method: 'POST' }),
    getCurrentUser: () => request('/api/auth/me')
};