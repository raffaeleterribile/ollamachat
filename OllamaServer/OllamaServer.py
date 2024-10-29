import asyncio
import ollama
from ollama import Client
from ollama import AsyncClient

print(">>> Response from the server")
response = ollama.chat(model='phi3:mini', messages=[
	{
		'role': 'user',
		'content': 'Why is the sky blue?',
	},
])
print(response['message']['content'], "\n")

print(">>> Streaming response from the server")
stream = ollama.chat(
		model='phi3:mini',
		messages=[{'role': 'user', 'content': 'Why is the sky blue?'}],
		stream=True,
)

for chunk in stream:
	print(chunk['message']['content'], end='', flush=True)
print("\n")

print(">>> Chat example")
response = ollama.chat(model='phi3:mini', messages=[{'role': 'user', 'content': 'Why is the sky blue?'}])
print(response['message']['content'], "\n")

print(">>> Generate with a prompt")
result = ollama.generate(model='phi3:mini', prompt='Why is the sky blue?')
print(result['response'], "\n")

print(">>> Custom client")
client = Client(host='http://localhost:11434')
result = client.chat(model='phi3:mini', messages=[
	{
		'role': 'user',
		'content': 'Why is the sky blue?',
	},
])
print(response['message']['content'], "\n")

print(">>> Async client")

async def chat():
	message = {'role': 'user', 'content': 'Why is the sky blue?'}
	asyncClient = AsyncClient()
	response = await asyncClient.chat(model='phi3:mini', messages=[message])
	print(response['message']['content'], "\n")

asyncio.run(chat())

print(">>> Async streaming client")

async def chat():
	message = {'role': 'user', 'content': 'Why is the sky blue?'}
	asyncClient = AsyncClient()
	async for part in await asyncClient.chat(model='phi3:mini', messages=[message], stream=True):
		print(part['message']['content'], end='', flush=True)
	print("\n")

asyncio.run(chat())

print(">>> Example using a ModelFile")
modelfile='''
FROM phi3:mini
SYSTEM You are mario from super mario bros.
'''

stream = ollama.create(model='example', modelfile=modelfile, stream=True)

for chunk in stream:
	print(chunk['status'], flush=True)
