import apiClient from './api';
import { serviceWrapper } from './serviceWrapper';

const callWithLoading = async (loadingRef, fn) => {
  if (loadingRef && typeof loadingRef === 'object' && 'value' in loadingRef) {
    loadingRef.value = true;
  }
  try {
    return await fn();
  } finally {
    if (loadingRef && typeof loadingRef === 'object' && 'value' in loadingRef) {
      loadingRef.value = false;
    }
  }
};

const _serviceMethods = {
  async getList() {
    const res = await apiClient.get('/teams');
    return res.data.data;
  },
  async search(srchobj) {
    const res = await apiClient.post('/teams/srch', srchobj);
    return res.data.data;
  },
  async get(id) {
    const res = await apiClient.get(`/teams/${id}`);
    return [res.data.data];
  },
  async add(team) {
    const res = await apiClient.post('/teams', team);
    return res.data.data;
  },
  async update(team) {
    const res = await apiClient.put(`/teams/${team.id}`, team);
    return res.data.data;
  },
  async delete(team) {
    const res = await apiClient.delete(`/teams/${team.id}`);
    return res.data.data;
  }
};

const _service = serviceWrapper('TeamService', _serviceMethods);

export const TeamService = {
  getList: (loading) => callWithLoading(loading, () => _service.getList()),
  search: (srchobj, loading) => callWithLoading(loading, () => _service.search(srchobj)),
  get: (id, loading) => callWithLoading(loading, () => _service.get(id)),
  add: (team, loading) => callWithLoading(loading, () => _service.add(team)),
  update: (team, loading) => callWithLoading(loading, () => _service.update(team)),
  delete: (team, loading) => callWithLoading(loading, () => _service.delete(team, { serviceName: 'TeamService' })),
  save(teams, loading) {
    return callWithLoading(loading, async () => {
      if (!teams.value || teams.value.length === 0) return;

      const savePromises = teams.value.map((team) => {
        // 변경되지 않은 항목은 무시 로직 나중에 추가 하자.
        if (team.id) {
          return _service.update(team);
        } else {
          return _service.add(team);
        }
      });

      await Promise.all(savePromises);
    });
  },
  deleteSelected(teamsToDelete, loading) {
    return callWithLoading(loading, async () => {
      if (!teamsToDelete || teamsToDelete.length === 0) return;

      const deletePromises = teamsToDelete.map((team) => _service.delete(team));

      await Promise.all(deletePromises);
    });
  }
};
