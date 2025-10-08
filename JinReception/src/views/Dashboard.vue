<script setup>
import { useLayout } from '@/layout/composables/layout';
import BestSellingWidget from '@/components/dashboard/BestSellingWidget.vue';
import NotificationsWidget from '@/components/dashboard/NotificationsWidget.vue';
import RecentSalesWidget from '@/components/dashboard/RecentSalesWidget.vue';
import RevenueStreamWidget from '@/components/dashboard/RevenueStreamWidget.vue';
import StatsWidget from '@/components/dashboard/StatsWidget.vue';
import CustomerDashboard from './CustomerDashboard.vue';
import RequestMonitor from './pages/reqs/RequestMonitor.vue';

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
