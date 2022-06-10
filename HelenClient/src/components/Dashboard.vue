<template>
    <div class="grid">
      <div class="col-12 lg:col-6 xl:col-3">
        <div class="card mb-0">
          <div class="flex justify-content-between mb-3">
            <div>
              <span class="block text-500 font-medium mb-3">Bugs</span>
              <div class="font-medium text-xl">{{ bugCount }}</div>
            </div>
            <div class="flex align-items-center justify-content-center bg-blue-100 border-round" style="width:2.5rem;height:2.5rem">           
              <FontAwesomeIcon :icon="bugIcon" class="text-blue-500" size="lg"/>
            </div>
          </div>
          <span class="text-green-500 font-medium">{{ newBug }} new </span>
          <span class="text-500">since last visit</span>
        </div>
		  </div>
    </div>
</template>

<script>

export default {
  components: {
    
  },
  setup(props, {emit}) {

    emit('routed', false);
    var bugCount = 3280;
    var newBug = 24;

    return {
      bugCount,
      newBug,
      bugIcon: ['fas', 'bug'],
    }
  },
}
</script>

<style>
.card {
  box-shadow: 0 8px 16px 0 rgb(133 133 133 / 28%) !important;
}
</style>