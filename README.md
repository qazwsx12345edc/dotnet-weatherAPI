* GetCities
	* 调用百度api 获取全国城市表和城市id 处理数据得到省市区全名存入数据库

* GetCityId 城市简称（如：杭州）查找城市id
* GetCityByFullName 省市区全程查找城市id

* RobotSend 钉钉机器人向群里投送消息

* GetWeather 两种格式 钉钉机器人@调用
	* 输入 城市名  => GetTodaysWeather
	* 输入 城市名/yyyy-mm-dd  => GetHistoryWeather
	
* GetTodaysWeather
	* GetCityId 然后调用百度api 结构存入数据库

* GetHistoryWeather
	* 查找数据库里的当天历史天气，对风力气压等求平均值
	定时任务会每小时将city表中monitor字段为1的城市查询天气并存入数据库
