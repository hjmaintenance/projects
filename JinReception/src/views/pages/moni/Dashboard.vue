<script setup>
import { useLayout } from '@/layout/composables/layout';
import CustomerDashboard from './CustomerDashboard.vue';
import RequestMonitor from './RequestMonitor.vue';

const { loginUser } = useLayout();
</script>

<template>
    <div v-if="loginUser.login_type === 'admin'">
        <RequestMonitor />
    </div>
    <div v-else>
        <CustomerDashboard />
    </div>
</template>
